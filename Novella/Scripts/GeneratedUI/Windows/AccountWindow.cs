using System.Collections;
using System.Collections.Generic;
using Scripts.UISystem;
using UnityEngine;
using Firebase.Firestore;
using Scripts.Managers;
using GeneratedUI;
using TMPro;
using Scripts;

public class AccountWindow : WindowController
{
	[SerializeField] private List<TextMeshProUGUI> uiTexts;
	[SerializeField] private TextMeshProUGUI name, email;
	private List<string> _uiTextsRu = new List<string>
		{"Ваши данные","Выйти"};
	private List<string> _uiTextsEng = new List<string>
		{"Your data","Sign out"};

	private void Start()
	{
		ref var user = ref GameManager.Instance.userData;
		switch (PlayerPrefs.GetString("language"))
		{
			case "eng":
				for (int i = 0; i < uiTexts.Count; i++)
				{
					uiTexts[i].text = _uiTextsEng[i];
				}
				name.text = "  Name:     " + user.username;
				email.text = "  E-mail:    " + user.email;
				break;
			case "rus":
				for (int i = 0; i < uiTexts.Count; i++)
				{
					uiTexts[i].text = _uiTextsRu[i];
				}
				name.text = "  Имя:       " + user.username;
				email.text = "  E-mail:   " + user.email;
				break;
		}
	}

	public void OnLeftButton()
    {
        WindowsManager.Instance.CreateWindow<SettingsWindow>();
        CloseWindow();
    }

    public void OnExitButton()
    {
        FirebaseManager.Instance.SignOut();
        WindowsManager.Instance.CreateScreen<LoginScreen>();
        WindowsManager.Instance.CloseAllWindows();
    }

}
