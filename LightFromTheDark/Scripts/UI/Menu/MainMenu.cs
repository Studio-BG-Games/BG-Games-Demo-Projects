using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject _listOfLevelsObj;
    [SerializeField] private GameObject _settingsMenuObj;
    [SerializeField] private GameObject _sceneObj;
    [SerializeField] private GameObject _buttonsObj;

    [Header("Компоненты кнопок")]
    [SerializeField] private AudioSource _clickSoundBtnPlay;
    [SerializeField] private AudioSource _clickSoundBtnSettings;
    [SerializeField] private AudioSource _clickSoundBtnExit;

    public void OnClickButtonPlay()
    {
        _clickSoundBtnPlay.Play();
        _sceneObj.SetActive(false);
        _buttonsObj.transform.position = new Vector3(-1000f, 0f, 0f);
        _listOfLevelsObj.SetActive(true);
    }

    public void OnClickButtonSettings()
    {
        _clickSoundBtnSettings.Play();
        _sceneObj.SetActive(false);
        _buttonsObj.transform.position = new Vector3(-1000f, 0f, 0f);
        _settingsMenuObj.SetActive(true);
    }

    public void OnClickButtonExit()
    {
        _clickSoundBtnExit.Play();
        Application.Quit();
    }
}
