using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyHandler : MonoBehaviour
{
	public static MoneyHandler Instance;
	public Action<int> OnMoneyUpdateAction;
	private int _money;

	private void Awake()
	{
		if(Instance == null)
		{
			Instance = this;
		}
		else
		{
			Destroy(gameObject);
		}
		DontDestroyOnLoad(gameObject);

		if (PlayerPrefs.HasKey("money"))
		{
			_money = PlayerPrefs.GetInt("money");
			StartCoroutine(UpdateMoney());
		}
	}

	IEnumerator UpdateMoney()
	{
		yield return new WaitForSeconds(0.5f);
		OnMoneyUpdateAction?.Invoke(_money);
	}

	public void AddMoneyToBank(int count)
	{
		_money += count;
		PlayerPrefs.SetInt("money", _money);
		OnMoneyUpdateAction?.Invoke(_money);
	}

	public bool PutMoneyToBank(int count)
	{
		if(_money >= count)
		{
			_money -= count;
			PlayerPrefs.SetInt("money", _money);
			OnMoneyUpdateAction?.Invoke(_money);
			return true;
		}
		else
		{
			Debug.Log($"Insufficient funds to buy product! Money: {_money} < Price: {count}");
			return false;
		}
		
	}
}
