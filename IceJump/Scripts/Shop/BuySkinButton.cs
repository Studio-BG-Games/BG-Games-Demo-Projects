using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuySkinButton : MonoBehaviour
{
	public Action OnBuySkinAction;
	public void BuySkinToPlayer()
	{
		OnBuySkinAction?.Invoke();
	}
}
