using System;
using System.Collections.Generic;
using Scripts.UISystem;
using TMPro;
using UnityEngine;
using Scripts;

public class AccountErrorWindow : WindowController
{
	[SerializeField] private List<TextMeshProUGUI> uiTexts;
	private List<string> _uiTextsRu = new List<string>
		{"Ошибка","На этот e-mail\nуже зарегистрирован аккаунт.\nВведите другой e-mail.","Ок"};
	private List<string> _uiTextsEng = new List<string>
		{"Error","An account is already\nregistered for this e-mail.\nEnter another e-mail.","Ok"};

	private void Start()
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
	}
}
