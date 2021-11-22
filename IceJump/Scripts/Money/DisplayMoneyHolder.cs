using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayMoneyHolder : MonoBehaviour
{
	[SerializeField] private Text _textMoney;
	private void Start()
	{
		MoneyHandler.Instance.OnMoneyUpdateAction += UpdateMoneyText;
	}

	private void UpdateMoneyText(int money)
	{
		_textMoney.text = money.ToString();
	}

	private void OnDestroy()
	{
		MoneyHandler.Instance.OnMoneyUpdateAction -= UpdateMoneyText;
	}
}
