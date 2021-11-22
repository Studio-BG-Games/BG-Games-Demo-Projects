using System.Collections;
using System.Collections.Generic;
using Scripts.UISystem;
using TMPro;
using UnityEngine;
using Scripts;

public class UsernameErrorWindow : WindowController
{
	[SerializeField] private List<TextMeshProUGUI> uiTexts;
	private List<string> _uiTextsRu = new List<string>
		{"Ошибка","Некорректное имя.\n" +
		"Имя должно содержать\n" +
		"больше 0 символов.","Ок"};
	private List<string> _uiTextsEng = new List<string>
		{"Error","Incorrect name.\n" +
		"Name must be more\n" +
		"than 0 characters","Ok"};

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
