using System.Collections;
using System.Collections.Generic;
using Scripts.UISystem;
using TMPro;
using UnityEngine;
using Scripts;

public class PackInfoWindow : WindowController
{
	[SerializeField] private List<TextMeshProUGUI> uiTexts;

	private void Start()
	{
		switch (PlayerPrefs.GetString("language"))
		{
			case "rus":
				uiTexts[0].text = "Паки(сверху - вниз) :\n" +
					"бриллиантовый <sprite=2>, золотой <sprite=1>\n" +
					"и серебряный <sprite=0>. Это наборы с\n" +
					"различным количеством\n" +
					"рубинов. Вам может попасть\n" +
					"количество рубинов соразмерно\n" +
					"его реальному ценнику, а может\n" +
					"выпасть значительно больше!\n" +
					"Испытайте свою удачу!";
				uiTexts[1].text = "Ок";
				break;
			case "eng":
				uiTexts[0].text = "Packs (top to bottom):\n" +
					"diamond <sprite=2>, gold <sprite=1>\n" +
					"and silver <sprite=0>. These are sets\n" +
					"with different amounts of rubies.\n" +
					"You can get the amount of rubies\n" +
					"commensurate with its real price\n" +
					"tag, or you can get much more!\n" +
					"Try your luck!";
				uiTexts[1].text = "Ok";
				break;
		}
	}
}
