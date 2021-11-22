using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
	public static InputHandler Instance;
	public bool IsDown = false;

	private void Awake()
	{
		Instance = this;
	}

	private void Update()
	{
		if (Input.GetMouseButton(0))
		{
			Debug.Log("Down");
			IsDown = true;
		}
		else
		{
			IsDown = false;
		}
	}
}
