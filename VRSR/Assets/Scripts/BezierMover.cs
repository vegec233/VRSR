using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BezierMover : MonoBehaviour
{
    public LineRender lineRender;
    public float moveSpeed = 2.0f;
    public float acceleration = 1.0f;
    public float invisDelay = 3f;

    private float currentSpd;
    private Vector3[] curvePoints;
    private bool isMoving = false;
    public bool ballTrigger = false;
    private Rigidbody rb;
    private bool hasUnfrozen = false;

    private RigidbodyConstraints initialConstraints;
    private MeshRenderer meshRenderer;

    private TrailRenderer trailRenderer;

    void Start()
    {
        currentSpd = moveSpeed;
        trailRenderer = GetComponent<TrailRenderer>();
        rb = GetComponent<Rigidbody>();
        meshRenderer = GetComponent<MeshRenderer>();
        initialConstraints = rb.constraints;

        curvePoints = lineRender.GetCurvePoints();

        if (curvePoints != null && curvePoints.Length > 0)
        {
            transform.position = curvePoints[0];
        }
    }

    void Update()
    {
        if (ballTrigger && !isMoving)
        {
            StartCoroutine(MoveAlongCurveCoroutine());
        }
    }

    void SetTrailAlpha(float alpha)
    {
        if (trailRenderer != null)
        {
            Color start = trailRenderer.startColor;
            Color end = trailRenderer.endColor;

            start.a = alpha;
            end.a = 0f;

            trailRenderer.startColor = start;
            trailRenderer.endColor = end;
        }
    }

    IEnumerator MoveAlongCurveCoroutine()
    {
        isMoving = true;

        rb.constraints = initialConstraints;
        hasUnfrozen = false;
        meshRenderer.enabled = false; // Start fully hidden

        while (ballTrigger)
        {
            curvePoints = lineRender.GetCurvePoints();
            currentSpd = moveSpeed;

            // ⏩ Instantly snap to curve start
            transform.position = curvePoints[0];
            yield return null; // wait one frame before showing

            // Now show it
            meshRenderer.enabled = true;
            trailRenderer.enabled = true;
            trailRenderer.time = 0.4f;

            // 🚶 Move along the curve
            for (int i = 0; i < curvePoints.Length; i++)
            {
                while (Vector3.Distance(transform.position, curvePoints[i]) > 0.05f)
                {
                    transform.position = Vector3.MoveTowards(transform.position,
                    curvePoints[i], currentSpd * Time.deltaTime);
                    currentSpd += acceleration * Time.deltaTime;
                    yield return null;
                }
            }

            if (!ballTrigger)
                break;

            // Hide again before next cycle
            // SetTrailAlpha(0f);
            trailRenderer.time = 0f;
            trailRenderer.enabled = false;
            meshRenderer.enabled = false;

            yield return new WaitForSeconds(0.1f); // optional pause
        }

        // 🧊 Final step: unfreeze + invis after delay
        if (!hasUnfrozen)
        {
            rb.constraints = RigidbodyConstraints.None;
            hasUnfrozen = true;
            StartCoroutine(InvisAfterDelay(invisDelay));
        }

        isMoving = false;
    }
    IEnumerator InvisAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        meshRenderer.enabled = false;
    }
}