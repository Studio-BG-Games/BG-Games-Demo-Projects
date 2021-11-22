using System.Collections;
using System.Collections.Generic;
using Scripts.UISystem;
using UnityEngine;
using Scripts;

public class ConfirmEmailWindow : WindowController
{
	[SerializeField] private List<TMPro.TextMeshProUGUI> uiTexts;
	private List<string> _uiTextsRu = new List<string>
		{"На ваш e-mail направлено письмо для\nподтверждения. Следуйте инструкции в письме.","Спасибо, что вы с нами!","Готово"};
	private List<string> _uiTextsEng = new List<string>
		{"A confirmation email has been sent\n to your e-mail. Follow the instructions\nin the e-mail.","Thank you for being with us!","Done"};

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
	public void OnAcceptButton()
    {
        WindowsManager.Instance.CreateWindow<SettingsWindow>();
        CloseWindow();
    }

}
