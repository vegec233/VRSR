using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BezierMover : MonoBehaviour
{
    public LineRender lineRender;   // Reference to LineRender script
    public float moveSpeed = 2.0f;
    private Vector3[] curvePoints;
    private int currentPointIndex = 0;
    private bool hasReachedEnd = false;

    void Start()
    {
        // Get the curve points from the LineRender script
        if (lineRender != null)
        {
            curvePoints = lineRender.GetCurvePoints();
            if (curvePoints.Length > 0)
            {
                transform.position = curvePoints[0];
            }
        }
        else
        {
            Debug.LogError("LineRender reference is not assigned.");
        }
    }

    void Update()
    {
        if (lineRender != null && !hasReachedEnd)
        {
            // Update the curve points dynamically if the curve changes
            curvePoints = lineRender.GetCurvePoints();

            if (curvePoints != null && curvePoints.Length > 0)
            {
                MoveAlongCurve();
            }
        }
    }

    void MoveAlongCurve()
    {
        // Move towards the next point
        transform.position = Vector3.MoveTowards(transform.position, curvePoints[currentPointIndex], moveSpeed * Time.deltaTime);

        // Check if the GameObject reached the current target point
        if (Vector3.Distance(transform.position, curvePoints[currentPointIndex]) < 0.1f)
        {
            currentPointIndex++;

            // Stop at the end of the curve
            if (currentPointIndex >= curvePoints.Length)
            {
                hasReachedEnd = true;
                currentPointIndex = curvePoints.Length - 1; // Stay at the last point
            }
        }
    }
}