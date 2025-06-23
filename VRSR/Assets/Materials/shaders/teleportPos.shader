Shader "Unlit/teleportPos"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BottomAlpha ("Bottom Alpha", Range(0,1)) = 1
        _TopAlpha ("Top Alpha", Range(0,1)) = 0
        _MinY ("Bottom Y", Float) = 0
        _MaxY ("Top Y", Float) = 1
        _Color ("Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float worldY : TEXCOORD1;
                UNITY_FOG_COORDS(2)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _BottomAlpha;
            float _TopAlpha;
            float _MinY;
            float _MaxY;
            float4 _Color;

            v2f vert (appdata v)
            {
                v2f o;
                float4 worldPos = mul(unity_ObjectToWorld, v.vertex);
                o.worldY = worldPos.y;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                // UNITY_TRANSFER_FOG(o, o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // fixed4 col = tex2D(_MainTex, i.uv);
                // col.a *= alpha;
                float t = saturate((i.worldY - _MinY) / (_MaxY - _MinY)); // normalized height
                float alpha = lerp(_BottomAlpha, _TopAlpha, t); // vertical fade
                

                fixed4 col = _Color;
                col.a *= alpha;

                // UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
