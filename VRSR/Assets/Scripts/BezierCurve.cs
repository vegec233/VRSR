using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bezier
{
    private static Vector3 CalculateFirstBezierPoint(float t, Vector3 p0, Vector3 p1)
    {
        float u = 1 - t;
        Vector3 p = u * p0;
        p += t * p1;
        return p;
    }

    private static Vector3 CalculateSecondBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2) 
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;

        Vector3 p = uu * p0;
        p += 2 * u * t * p1;
        p += tt * p2;
        return p;
    }

    private static Vector3 CalculateThirdPowerBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float ttt = tt * t;
        float uuu = uu * u;

        Vector3 p = uuu * p0;
        p += 3 * t * uu * p1;
        p += 3 * tt * u * p2;
        p += ttt * p3;

        return p;
    }

    public static Vector3[] GetPowerBezierList(Vector3[] PosArr, int segmentNum)
    {
        Vector3[] path = new Vector3[segmentNum];
        for (int i = 1; i <= segmentNum; i++)
        {
            float t = i / (float)segmentNum;
            Vector3 pixel = BezierRatio(PosArr, t);
            path[i - 1] = pixel;
        }
        return path;
    }

    public static Vector3 BezierRatio(Vector3[] posArr, float t)
    {
        Vector3 localPos = Vector3.zero;
        int n = posArr.Length - 1;
        for (int i = 0; i <= n; i++)
        {
            Vector3 item = posArr[i];
            int index = i;
            double m1 = fact(n);
            double m2 = fact(index);
            double m3 = fact(n - index);
            double m4 = Mathf.Pow((1 - t), n - index);
            double m5 = Mathf.Pow(t, index);
            double m6 = (m1 / m2 / m3) * m4 * m5;
            localPos += new Vector3((float)m6 * item.x, (float)m6 * item.y, (float)m6 * item.z);
        }
        return localPos;
    }

    public static double fact(int n)
    {
        if (n == 0) return 1.0f;
        double f = 1;
        for (int i = 1; i <= n; i++)
        {
            f *= i;
        }
        return f;
    }
 
}


