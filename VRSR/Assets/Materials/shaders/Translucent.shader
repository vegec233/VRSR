Shader "Unlit/Translucent"
{
    Properties
    {
        _MainTex ("Main Tex", 2D) = "white" {}
        _BumpMap ("Bump Map", 2D) = "bump" {}
        _RimColor ("Rim Color", Color) = (0.46, 0.0, 1.0, 0.0)
        _RimPower ("Rim Power", Range(0.0, 3.0)) = 0.5
        _Brightness ("Brightness", Range(0.0, 5.0)) = 1.0
    }

    SubShader
    {
        Tags
        {
            "RenderType" = "Transparent"
            "Queue" = "Transparent"
            "IgnoreProject" = "True"
        }

        Pass
        {
            ZWrite On
            ColorMask 0
        }

        Pass
        {
            Tags { "LightMode"="ForwardBase" }
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            sampler2D _BumpMap;
            float4 _RimColor;
            float _RimPower;
            float _Brightness;

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 uv : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                UNITY_VERTEX_INPUT_INSTANCE_ID       
                UNITY_VERTEX_OUTPUT_STEREO           
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 viewDir : TEXCOORD1;
                float3 worldNormal : TEXCOORD2;
                float3 worldTangent : TEXCOORD3;
                float3 worldBinormal : TEXCOORD4;
                UNITY_FOG_COORDS(5)
            };

            v2f vert(appdata v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_TRANSFER_INSTANCE_ID(v, o);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

                o.pos = UnityObjectToClipPos(v.vertex);
                float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                float3 n = UnityObjectToWorldNormal(v.normal);
                float3 t = UnityObjectToWorldDir(v.tangent.xyz);
                float3 b = cross(n, t) * v.tangent.w;

                o.viewDir = _WorldSpaceCameraPos.xyz - worldPos;
                o.worldNormal = n;
                o.worldTangent = t;
                o.worldBinormal = b;
                o.uv = v.uv;

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 uv = i.uv;

                // Sample and convert to grayscale (if needed)
                float4 basecol = tex2D(_MainTex, uv);
                float gray = dot(basecol.rgb, float3(0.3, 0.59, 0.11));

                // Normal mapping
                float3 normalTex = UnpackNormal(tex2D(_BumpMap, uv));
                float3x3 TBN = float3x3(normalize(i.worldTangent), normalize(i.worldBinormal), normalize(i.worldNormal));
                float3 worldNormal = normalize(mul(normalTex, TBN));

                // Rim lighting
                float3 viewDir = normalize(i.viewDir);
                float rim = 1.0 - saturate(dot(viewDir, worldNormal));
                float3 rimLight = _RimColor.rgb * pow(rim, _RimPower) * _Brightness;

                // Final alpha from rim intensity
                float alpha = (rimLight.r + rimLight.g + rimLight.b) / 3.0;
                rimLight = saturate(rimLight);

                return float4(rimLight, alpha);
            }
            ENDCG
        }
    }

    FallBack "Diffuse"
}