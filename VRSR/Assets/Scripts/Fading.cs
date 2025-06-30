using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fading : MonoBehaviour
{
    [SerializeField] private float fadeDuration = 5f;
    [SerializeField] private float waitAfterFadeIn = 2f; // 👈 new wait time after fade in
    [SerializeField] private bool fadeIn = false;

    [SerializeField] private Material material; // Material using _MainColor
    [Range(0f, 1f)] public float alpha = 1f;

    private Coroutine fadeRoutine;

    void Start()
    {

    }

    public void FadeIn()
    {
        StartFade(1f, true);  // Fully visible, and trigger auto fade out
    }

    public void FadeOut()
    {
        StartFade(0f, false);  // Fully transparent, no further action
    }

    private void StartFade(float targetAlpha, bool waitThenFadeOut)
    {
        if (fadeRoutine != null)
            StopCoroutine(fadeRoutine);
        fadeRoutine = StartCoroutine(FadeMaterialAlpha(targetAlpha, waitThenFadeOut));
    }

    private IEnumerator FadeMaterialAlpha(float targetAlpha, bool waitThenFadeOut)
    {
        Color color = material.GetColor("_MainColor");
        float startAlpha = color.a;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / fadeDuration;
            color.a = Mathf.Lerp(startAlpha, targetAlpha, t);
            material.SetColor("_MainColor", color);
            yield return null;
        }

        // Ensure final alpha is exactly set
        color.a = targetAlpha;
        material.SetColor("_MainColor", color);

        // Wait, then auto fade out if requested
        if (waitThenFadeOut && targetAlpha == 1f)
        {
            yield return new WaitForSeconds(waitAfterFadeIn);
            StartFade(0f, false); // fade out, no further chaining
        }

        fadeRoutine = null;
    }
}
