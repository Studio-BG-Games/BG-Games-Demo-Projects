using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerToCart : MonoBehaviour
{
	public Action<int> OnTriggerAction;
	private enum TypeTriggerCart
	{
		left,
		right
	}
	[SerializeField] private TypeTriggerCart _type;

	public void OnTriggerEnter2D()
	{
		OnTriggerAction?.Invoke((int)_type);

	}

}
