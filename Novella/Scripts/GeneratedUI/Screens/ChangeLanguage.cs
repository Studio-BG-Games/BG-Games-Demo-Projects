using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChangeLanguage : MonoBehaviour
{
    [SerializeField] private Image engButton;
    [SerializeField] private Image rusButton;
    [SerializeField] private GameObject languageWindow;
    [SerializeField] private GameObject loginPanel;
    [SerializeField] private List<TextMeshProUGUI> uiTexts;
    private List<string> _uiTextsRu = new List<string>
    {"Заполните данные","Пароль","Войти","Зарегистрироваться"};
    private List<string> _uiTextsEng = new List<string>
    {"Fill in the data","Password","Sign in","Registration"};

    private void Start()
    {
        if (PlayerPrefs.GetString("LanguageSelection") == "true")
        {
            switch (PlayerPrefs.GetString("language"))
            {
                case "eng":
                    for (int i = 0; i < uiTexts.Count; i++)
                    {
                        uiTexts[i].text = _uiTextsEng[i];
                    }
                    break;
                case "rus":
                    for (int i = 0; i < uiTexts.Count; i++)
                    {
                        uiTexts[i].text = _uiTextsRu[i];
                    }
                    break;
            }

            languageWindow.SetActive(false);
            loginPanel.SetActive(true);

        }
        else
        {
            PlayerPrefs.SetString("language", "rus");
            for (int i = 0; i < uiTexts.Count; i++)
            {
                uiTexts[i].text = _uiTextsRu[i];
            }
        }
    }
    
    public void ChooseLanguage(string language)
    {
        switch (language)
        {
            case "eng":
                PlayerPrefs.SetString("language", "eng");
                rusButton.color = Color.gray;
                engButton.color = Color.white;
                for (int i = 0; i < uiTexts.Count; i++)
                {
                    uiTexts[i].text = _uiTextsEng[i];
                }
                break;
            case "rus":
                PlayerPrefs.SetString("language", "rus");
                engButton.color = Color.gray;
                rusButton.color = Color.white;
                for (int i = 0; i < uiTexts.Count; i++)
                {
                    uiTexts[i].text = _uiTextsRu[i];
                }
                break;
        }
    }

    public void CloseLanguageWindow()
    {
        PlayerPrefs.SetString("LanguageSelection","true");
        languageWindow.SetActive(false);
        loginPanel.SetActive(true);
    }   
}
