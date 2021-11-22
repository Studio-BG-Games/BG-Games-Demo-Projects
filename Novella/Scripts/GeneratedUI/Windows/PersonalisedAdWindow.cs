using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.UISystem;
using TMPro;
using Scripts;

public class PersonalisedAdWindow : WindowController
{
	[SerializeField] private List<TextMeshProUGUI> uiTexts;
	private List<string> _uiTextsRu = new List<string>
		{"Включить персонализированную рекламу?","«Бесконечные фантазии» может персонализировать\n" +
		"Вашу рекламу с помощью сервиса Appodeal. Сервис\n" +
		"Appodeal и его партнеры могут собирать и\n" +
		"обрабатывать персональные данные, такие как\n" +
		"идентификаторы устройств, данные о местоположении,\n" +
		"а также демографические данные и данные\n" +
		"о ваших интересах, для предоставления рекламных\n" +
		"услуг с учётом Ваших предпочтений. Согласившись\n" +
		"на эту улучшенную рекламную функцию, Вы\n" +
		"будете видеть рекламу, которая, по мнению\n" +
		"Appodeal и ее партнеров, наиболее\n" +
		"актуальна для вас.","Я понимаю, что я все равно буду видеть рекламу,\n" +
		"но она может не соответствовать моим интересам.","Да, включить","Политика конф-ти","Нет, спасибо"};
	private List<string> _uiTextsEng = new List<string>
		{"Do you want to turn on personalized ads?","Infinite Fantasies personalizes your advertising experience\n" +
		"using Appodeal. Appodeal and its partners may\n" +
		"collect and process personal data such as\n" +
		"device identifiers, location data,and other\n" +
		"demographic and interest data to provide an\n" +
		"advertising experience tailored to you. By\n" +
		"consenting to this improved ad experience,\n" +
		"you'll see ads that Appodeal and its\n" +
		"partners believe are more\n" +
		"relevant to you.","I understand that I will still see ads,\n" +
		"but they may not be as relevant to my interests.","Yes, I do","Privacy Policy","No, thanks"};

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

	public void OpenLink(string link)
	{
		Application.OpenURL(link);
	}
}

