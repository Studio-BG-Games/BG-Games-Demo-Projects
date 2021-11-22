using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    [SerializeField] private Toggle _toggleEffects;
    [SerializeField] private Toggle _toggleMusic;

    private void Start()
    {
        _toggleEffects.isOn = AudioManager.Instance.IsEffectsMute;
        _toggleMusic.isOn = AudioManager.Instance.IsMusicMute;
    }

    public void OnClickButtonPause()
    {
        Time.timeScale = 0;
        gameObject.SetActive(true);
    }

    public void OnClickButtonMenu()
    {
        FindObjectOfType<SoundLevel>().GetComponent<SoundLevel>().DestroyObject();
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
    public void OnClickToggleEffects(bool value)
    {
        AudioManager.Instance.IsEffectsMute = value;
    }
    public void OnClickToggleMusic(bool value)
    {
        AudioManager.Instance.IsMusicMute = value;
    }
    public void OnClickButtonContinue()
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }


    public void OnClickSkipLevel()
    {
        GameObject player = GameObject.FindWithTag("Player");
        GameObject finishZone = FindObjectOfType<FinishZone>().gameObject;
        player.transform.position = finishZone.transform.position;
    }

}
