using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowController : MonoBehaviour
{
	[SerializeField] private Window _window;

	private void Start()
	{
		if (WindowHandler.Instance != null)
		{
			WindowHandler.Instance.SetWindow(_window);
		}
	}
	private void OnEnable()
	{
		if(WindowHandler.Instance != null)
		{
			WindowHandler.Instance.SetWindow(_window);
		}
	
	}
}
