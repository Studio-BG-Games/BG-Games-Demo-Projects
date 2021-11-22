using System.Collections;
using System.Collections.Generic;
using Scripts.UISystem;
using UnityEngine;
using TMPro;
using Scripts;

public class PasswordErrorWindow : WindowController
{
    [SerializeField] private TextMeshProUGUI uiText1, uiText2, uiText3;

    private void Start() 
    {
        switch (PlayerPrefs.GetString("language"))
		{
			case "rus":
				uiText1.text = "Ошибка";
                uiText2.text = "Некорректный пароль.\nПароль должен содержать\n8 и больше  символов.";
                uiText3.text = "Ок";
				break;
			case "eng":			
				uiText1.text = "Error";
                uiText2.text = "Invalid password.\nPassword must contain\n8 or more characters.";
                uiText3.text = "Ok";
				break;
		}
    }
}
