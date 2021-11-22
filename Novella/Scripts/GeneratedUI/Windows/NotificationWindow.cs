using System.Collections;
using System.Collections.Generic;
using Scripts.UISystem;
using TMPro;
using UnityEngine;
using Scripts;

public class NotificationWindow : WindowController
{
	[SerializeField] private List<TextMeshProUGUI> uiTexts;

	private void Start()
	{
		switch (PlayerPrefs.GetString("language"))
		{
			case "rus":
				uiTexts[0].text = "Включите наши уведомления, чтобы\n" +
					"не пропустить обновления,\n" +
					"ежедневные награды и возможность\n" +
					"накопить валюту.";
				uiTexts[1].text = "Коснитесь, чтобы продолжить";
				break;
			case "eng":
				uiTexts[0].text = "Turn on our notifications\n" +
					"so you don't miss out on updates,\n" +
					"daily rewards, and the\n" +
					"ability to save up currency.";
				uiTexts[1].text = "Tap to continue";
				break;
		}
	}

}
