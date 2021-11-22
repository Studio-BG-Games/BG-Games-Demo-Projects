using System;
using System.Collections;
using System.Collections.Generic;
using Scripts.UISystem;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Scripts;

public class NoMoneyPopupWindow : WindowController
{
    [SerializeField] private float closeAfterTimer = 2f;
    [SerializeField] private TextMeshProUGUI text;

    private void Start()
    {
		switch (PlayerPrefs.GetString("language"))
		{
			case "rus":
				text.text = "Денег нет";
				break;
			case "eng":
				text.text = "No money";
				break;
		}
		Sequence sequence = DOTween.Sequence();
        sequence.AppendInterval(closeAfterTimer);
        sequence.AppendCallback(CloseWindow);
    }
}
