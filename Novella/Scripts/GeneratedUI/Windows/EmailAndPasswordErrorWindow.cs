using System.Collections;
using System.Collections.Generic;
using Scripts.UISystem;
using TMPro;
using UnityEngine;
using Scripts;

public class EmailAndPasswordErrorWindow : WindowController
{
	[SerializeField] private List<TextMeshProUGUI> uiTexts;
	private List<string> _uiTextsRu = new List<string>
		{"Ошибка","Неверно введенный e-mail\nили пароль.","Ок"};
	private List<string> _uiTextsEng = new List<string>
		{"Error","Incorrectly entered e-mail\nor password.","Ok"};

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
