using System.Collections;
using System.Collections.Generic;
using Scripts.UISystem;
using TMPro;
using UnityEngine;
using Scripts;

public class ErrorWindow : WindowController
{
	[SerializeField] private List<TextMeshProUGUI> uiTexts;
	private List<string> _uiTextsRu = new List<string>
		{"Ошибка","Некорректно введенные данные,\nпопробуйте еще раз." +
		"\n\nНапоминаем,\n1) Пароль должен содержать от 8 символов\n2) Имя должно содеражать больше 0 символов","Ок"};
	private List<string> _uiTextsEng = new List<string>
		{"Error","Incorrectly entered data,\ntry again.\n\nRemind,\n" +
		"1) Password must contain at least 8 characters\n2) Name must be more than 0 characters","Ok"};

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
