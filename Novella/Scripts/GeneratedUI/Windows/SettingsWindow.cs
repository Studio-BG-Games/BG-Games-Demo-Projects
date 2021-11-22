using System.Collections;
using System.Collections.Generic;
using Scripts.Managers;
using Scripts.UISystem;
using GeneratedUI;
using TMPro;
using UnityEngine;
using Scripts;

public class SettingsWindow : WindowController
{
    [SerializeField] private List<TextMeshProUGUI> uiTexts;
    [SerializeField] private GameObject soundOn, soundOff;
    private List<string> _uiTextsRu = new List<string>
        {"Звук","Язык","Русский","Конф.","Условия","Реклама","Данные игрока","Закрыть","Выход из игры", "Восстановить покупки"};
    private List<string> _uiTextsEng = new List<string>
        {"Sound","Language","English","Privacy","Terms","Ad options","Player data","Close","Exit game", "Restore Purchases"};

    
    private void Start()
    {
        if (PlayerPrefs.GetString("language") == "rus")
        {
            for (int i = 0; i < uiTexts.Count; i++)
            {
                uiTexts[i].text = _uiTextsRu[i];
            }
        }
        else
        {
            for (int i = 0; i < uiTexts.Count; i++)
            {
                uiTexts[i].text = _uiTextsEng[i];
            }
        }

        if (PlayerPrefs.GetString("Volume") == "false")
        {
            soundOff.SetActive(true);
        }
        else
        { 
            soundOn.SetActive(true);
        }
        
    }

    public void OpenQuitGameWindow()
    {
        WindowsManager.Instance.CreateWindow<QuitGameWindow>();
    }

    public void OnLoginButton()
    {
        WindowsManager.Instance.CreateWindow<AccountWindow>();
        CloseWindow();
    }

    public void OnAdButton()
    {
        WindowsManager.Instance.CreateWindow<PersonalisedAdWindow>();
        CloseWindow();
    }

    public void OnLanguageButton()
    {
       
        if (PlayerPrefs.GetString("language") == "rus")
        {
            PlayerPrefs.SetString("language", "eng");
            for (int i = 0; i < uiTexts.Count; i++)
            {
                uiTexts[i].text = _uiTextsEng[i];
            }
        }
        else
        {
            PlayerPrefs.SetString("language", "rus");
            for (int i = 0; i < uiTexts.Count; i++)
            {
                uiTexts[i].text = _uiTextsRu[i];
            }
        }

        WindowsManager.Instance.SearchForScreen<MenuScreen>().UpdateCoverSprite();
    }

    public void OpenLink(string link)
    {
        Application.OpenURL(link);
    }

    public void OnSoundButton()
    {
        if (PlayerPrefs.GetString("Volume") != "false")
        {
            PlayerPrefs.SetString("Volume", "false");
            soundOff.SetActive(true);
            soundOn.SetActive(false);
            GameManager.Instance.soundManager.Mute(true);
        }
        else
        {
            PlayerPrefs.SetString("Volume", "true");
            soundOn.SetActive(true);
            soundOff.SetActive(false);
            GameManager.Instance.soundManager.Mute(false);
        }
    }
}
