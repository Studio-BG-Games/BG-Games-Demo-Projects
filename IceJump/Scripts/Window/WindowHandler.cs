using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowHandler : MonoBehaviour
{
	public static WindowHandler Instance;
	public Window CurrentWindow;

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
	}

	public void SetWindow(Window window)
	{
		CurrentWindow = window;
		if(CurrentWindow == Window.menu)
		{
			MoneyHandler.Instance.AddMoneyToBank(0);
		}
	}
}
