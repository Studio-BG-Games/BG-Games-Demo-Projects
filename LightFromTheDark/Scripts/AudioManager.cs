using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    static public AudioManager Instance = null;
    public AudioMixerGroup MixerEffects;
    public AudioMixerGroup MixerMusic;
    public AudioMixerGroup MixerUI;


    private float effectsVolue;
    private float musicVolue;

    public float EffectsVolume
    {
        set
        {
            effectsVolue = value;
            MixerEffects.audioMixer.SetFloat("EffectsVolume", value);
            MixerUI.audioMixer.SetFloat("UIVolume", value);
            PlayerPrefs.SetFloat("EffectsVolume", value);

            if (isEffectsMute == true)
                isEffectsMute = false;

        }
        get { return effectsVolue; }
    }

    public float MusicVolume
    {
        set
        {
            musicVolue = value;
            MixerEffects.audioMixer.SetFloat("MusicVolume", value);
            PlayerPrefs.SetFloat("MusicVolume", value);

            if (isMusicMute == true)
                isMusicMute = false;
        }
        get { return musicVolue; }
    }

    private bool isEffectsMute;
    private bool isMusicMute;

    public bool IsEffectsMute 
    { 
        set
        {
            isEffectsMute = value;
            if (value == false)
            {
                MixerEffects.audioMixer.SetFloat("EffectsVolume", effectsVolue);
                MixerUI.audioMixer.SetFloat("UIVolume", effectsVolue);
            }
            else
            {
                MixerEffects.audioMixer.SetFloat("EffectsVolume", -80);
                MixerUI.audioMixer.SetFloat("UIVolume", -80);
            }
        }
        get { return isEffectsMute; }
    }

    public bool IsMusicMute
    {
        set
        {
            isMusicMute = value;
            if (value == false)
                MixerMusic.audioMixer.SetFloat("MusicVolume", musicVolue);
            else
                MixerMusic.audioMixer.SetFloat("MusicVolume", -80);
        }
        get { return isMusicMute; }
    }

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeManager();
        }
        else
        { 
            Destroy(gameObject);
        }
    }


    private void InitializeManager()
    {
        effectsVolue = PlayerPrefs.GetFloat("EffectsVolume");
        musicVolue = PlayerPrefs.GetFloat("MusicVolume");
        MixerEffects.audioMixer.SetFloat("EffectsVolume", effectsVolue);
        MixerUI.audioMixer.SetFloat("UIVolume", effectsVolue);
        MixerEffects.audioMixer.SetFloat("MusicVolume", musicVolue);
    }
}