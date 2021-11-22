using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private GameObject _sceneObj;
    [SerializeField] private GameObject _buttonsObj;

    [Header("UI")]
    [SerializeField] private Slider _sliderEffectsVolume;
    [SerializeField] private Slider _sliderMusicVolume;

    private void Awake()
    {
        _sliderEffectsVolume.value = AudioManager.Instance.IsEffectsMute == true ? -80 : AudioManager.Instance.EffectsVolume;
        _sliderMusicVolume.value = AudioManager.Instance.IsMusicMute == true ? -80 : AudioManager.Instance.MusicVolume;
    }

    public void SetVolumeEffects(float value)
    {
        AudioManager.Instance.EffectsVolume = value;
    }

    public void SetVolumeMusic(float value)
    {
        AudioManager.Instance.MusicVolume = value;
    }

    public void OnClickButtonClose()
    {
        _sceneObj.SetActive(true);
        _buttonsObj.transform.position = Vector3.zero;
        gameObject.SetActive(false);
    }
}
