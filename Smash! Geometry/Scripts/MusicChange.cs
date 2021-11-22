using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public class MusicChange : MonoBehaviour
{
    public AudioClip otherClip;
     AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        Timing.RunCoroutine(ChangeMusic(), "music");
    }

   /* void Update()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.clip = otherClip;
            audioSource.Play();
        }
    }*/

    IEnumerator<float> ChangeMusic()
    {
        yield return Timing.WaitForSeconds(124);
        audioSource.clip = otherClip;
        audioSource.Play();
    }
}
