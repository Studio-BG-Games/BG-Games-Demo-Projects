using System.Collections;
using System.Collections.Generic;
using Scripts.UISystem;
using TMPro;
using UnityEngine;
using Scripts;

public class NoAdWindow : WindowController
{
	[SerializeField] private List<TextMeshProUGUI> uiTexts;
	private List<string> _uiTextsRu = new List<string>
		{"Вы можете НАВСЕГДА\nизбавиться от рекламы.","Получить","Восстановить","Отмена"};
	private List<string> _uiTextsEng = new List<string>
		{"You can get rid\nof ads FOREVER.","Get","Restore","Cancel"};

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

	public void DisableAdsForever()
	{
		PlayerPrefs.SetInt("NO_ADS", 1);
	}

	public void OpenLink(string link)
	{
		Application.OpenURL(link);
	}
}
