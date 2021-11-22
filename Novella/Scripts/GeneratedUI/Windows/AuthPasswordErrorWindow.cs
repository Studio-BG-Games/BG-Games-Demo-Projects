using System.Collections;
using System.Collections.Generic;
using Scripts.UISystem;
using UnityEngine;
using TMPro;
using Scripts;

public class AuthPasswordErrorWindow : WindowController
{
    [SerializeField] private TextMeshProUGUI uiText1, uiText2, uiText3;

    private void Start() 
    {
        switch (PlayerPrefs.GetString("language"))
		{
			case "rus":
				uiText1.text = "Ошибка";
                uiText2.text = "Неверный пароль.\nПожалуйста, попробуйте снова.";
                uiText3.text = "Ок";
				break;
			case "eng":			
				uiText1.text = "Error";
                uiText2.text = "Wrong password.\nPlease try again.";
                uiText3.text = "Ok";
				break;
		}
    }
}
