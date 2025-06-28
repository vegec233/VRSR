using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoicePlay : MonoBehaviour
{
    public AudioClip[] clips;
    public AudioSource audioSource;
    public bool playTrigger = false;

    private int currentClipIndex = 0;
    private bool isPlaying = false;

    void Start()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (playTrigger && !isPlaying && currentClipIndex < clips.Length)
        {
            StartCoroutine(PlayClipOnce(clips[currentClipIndex]));
            currentClipIndex++;
        }

        // Reset trigger to avoid replaying on the same true value
        if (!playTrigger)
        {
            isPlaying = false;
        }
    }

    IEnumerator PlayClipOnce(AudioClip clip)
    {
        isPlaying = true;
        audioSource.clip = clip;
        audioSource.Play();

        yield return new WaitForSeconds(clip.length);

        // Ready for next trigger
        isPlaying = false;
        playTrigger = false;
    }
}
