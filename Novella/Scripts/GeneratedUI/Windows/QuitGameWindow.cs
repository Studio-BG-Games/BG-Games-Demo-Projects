using Scripts.UISystem;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Scripts;

public class QuitGameWindow : WindowController
{
	[SerializeField] private List<TextMeshProUGUI> uiTexts;
	private List<string> _uiTextsRu = new List<string>
		{"Покинуть игру? Вы уверены?","Да","Нет"};
	private List<string> _uiTextsEng = new List<string>
		{"Do you want to leave the game?","Yes","No"};

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
	public void OnYesButton()
    {
        Application.Quit();
    }

}
