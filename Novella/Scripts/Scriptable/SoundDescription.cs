using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundDescription", menuName = "ScriptableObjects/SoundDescription", order = 1)]
public class SoundDescription : ScriptableObject
{
    public float SoundLength => sound.length;
    public string key => sound.name;
    [Range(0f, 1f)] public float volume = 1f;
    public AudioClip sound;
    //public AudioSource audioSource;

    /*
    public void PlaySound()
    {
        audioSource.Play();
    }
    */

    /*public void StopSound()
    {
        if (audioSource != null)
            Destroy(audioSource.gameObject);
    }*/


    /*public AudioSource InitAudioSource()
    {
        var soundPrefab = Resources.Load<GameObject>("SoundSource");
        audioSource = Instantiate(soundPrefab).GetComponent<AudioSource>();
        audioSource.clip = sound;
        audioSource.volume = 0;
        return audioSource;
    }*/
}