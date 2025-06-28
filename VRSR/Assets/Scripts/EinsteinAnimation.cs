using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EinsteinAnimation : MonoBehaviour
{
    public Animator animator;
    public AudioSource einsAudioSrc;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (einsAudioSrc.isPlaying)
        {
            animator.SetBool("startTalking", true);
        }
        else
        {
            animator.SetBool("startTalking", false);
        }
    }
}
