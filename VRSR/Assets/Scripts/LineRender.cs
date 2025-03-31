using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(LineRenderer))]
public class LineRender : MonoBehaviour
{
    public Transform[] controlPoints;
    private LineRenderer lineRenderer;
    private int layerOrder = 0;
    private int _segmentNum = 50;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.sortingLayerID = layerOrder;
    }

    void Update()
    {
        DrawThreePowerCurve();
    }

    void DrawThreePowerCurve()
    {
        Vector3[] localpos = new Vector3[controlPoints.Length];
        for (int i = 0; i < controlPoints.Length; i++)
        {
            localpos[i] = controlPoints[i].position;
        }

        Vector3[] points3 = Bezier.GetPowerBezierList(localpos, _segmentNum);
        lineRenderer.positionCount = _segmentNum;
        lineRenderer.SetPositions(points3);
    }

    public Vector3[] GetCurvePoints()
    {
        Vector3[] localpos = new Vector3[controlPoints.Length];
        for (int i = 0; i < controlPoints.Length; i++)
        {
            localpos[i] = controlPoints[i].position;
        }
        return Bezier.GetPowerBezierList(localpos, _segmentNum);
    }
}
