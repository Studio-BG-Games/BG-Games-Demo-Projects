using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundManager", menuName = "ScriptableObjects/SoundManager", order = 1)]
public class SoundManager : ScriptableObject
{
    [SerializeField] private List<SoundDescription> soundBase = new List<SoundDescription>();
    private SoundDescription _currentSound;
    private AudioSource _audioSource;

    private List<SoundDescription> GetSounds(string[] soundKeys)
    {
        var sounds = new List<SoundDescription>();

        foreach (var soundKey in soundKeys)
        {
            foreach (var sound in soundBase.Where(sound => sound.key == soundKey))
            {
                sounds.Add(sound);
                break;
            }
        }

        return sounds;
    }

    public string PlaySounds(string[] soundKeys, float inTime, float outTime)
    {
        var sounds = GetSounds(soundKeys);

        if (soundKeys.Length == 1)
        {
            PlaySound(sounds[0], inTime, outTime);
        }
      
        return soundKeys[0];
    }

    private void PlaySound(SoundDescription sound, float inTime, float outTime)
    {
        if (sound == null)
        {
            Debug.LogWarning("Sound not found");
            return;
        }
        void OnComplete() => PlayNextSound(sound, inTime);
        StopCurrentSound(outTime, OnComplete);
    }

    public void DoNull(float inTime)
    {
        _audioSource.DOFade(0, inTime);
    }
    private void PlayNextSound(SoundDescription sound, float inTime)
    {
        if (_audioSource == null)
            InitAudioSource();
      
        _audioSource.clip = sound.sound;
        _audioSource.loop = true;
        _audioSource.Play();
        _audioSource.DOFade(1f, inTime);
       
    }

    private void InitAudioSource()
    {
        var soundPrefab = Resources.Load<GameObject>("SoundSource");
        _audioSource = Instantiate(soundPrefab).GetComponent<AudioSource>();
    }

    public void StopCurrentSound(float outTime, Action OnComplete = null)
    {
        if (_audioSource == null)
        {
            OnComplete?.Invoke();
            return;
        }

        var sequence = DOTween.Sequence();
        sequence.Append(_audioSource.DOFade(0f, outTime));
        sequence.OnComplete(() => OnComplete?.Invoke());
    }

    public void PlayClick()
    {
        foreach (var soundKey in soundBase)
        {
            foreach (var sound in soundBase.Where(sound => sound.key == soundKey.key))
            {
                if (soundKey.name == "Sound 34")
                {
                    _audioSource.PlayOneShot(sound.sound);
                }
            }         
        }  
    }

    public void Mute(bool mod)
    {
        if(_audioSource==null)//Artem рандомно часто краш
        {
            return;
        }
        _audioSource.mute = mod;
    }

    public bool CheckAudioSource()
    {
        return _audioSource != null;
    }
}