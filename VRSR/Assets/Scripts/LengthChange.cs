using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LengthChange : MonoBehaviour
{
    public Transform targetPoint;     // Destination point
    public float moveSpeed = 2f;      // Movement speed
    public float finalXScale = 0.75f; // Target X scale (only X changes)

    private Vector3 startPos;
    private Vector3 targetPos;
    private float initialXScale;
    private Vector3 baseScale;
    private bool isMoving = false;

    void Start()
    {
        startPos = transform.position;
        targetPos = targetPoint.position;
        baseScale = transform.localScale;
        initialXScale = baseScale.x;

        StartCoroutine(MoveAndShrinkCoroutine());
    }

    IEnumerator MoveAndShrinkCoroutine()
    {
        isMoving = true;
        float distance = Vector3.Distance(startPos, targetPos);
        float traveled = 0f;

        while (traveled < distance)
        {
            float step = moveSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPos, step);
            traveled += step;

            // Shrink X only based on % of travel
            float t = traveled / distance;
            float newX = Mathf.Lerp(initialXScale, finalXScale, t);
            transform.localScale = new Vector3(newX, baseScale.y, baseScale.z);

            yield return null;
        }

        // Final correction
        transform.position = targetPos;
        transform.localScale = new Vector3(finalXScale, baseScale.y, baseScale.z);
        isMoving = false;
    }
}
