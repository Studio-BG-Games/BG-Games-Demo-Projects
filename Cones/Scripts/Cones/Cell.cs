using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
	public int Id;
	private Transform _transform;
	public Transform Transform { get{ return _transform; } }
	public Circle CircleBusy;
	public Circle StartCircleBusy;
	public bool IsBusy;

	private void Start()
	{
		_transform = GetComponent<Transform>();
		if(CircleBusy != null)
		{
			StartCircleBusy = CircleBusy;
			CircleBusy.transform.position = transform.position;
			CircleBusy.SetStartPosition();
		}
	}

	public void Restart()
	{
		if(StartCircleBusy != null)
		{
			CircleBusy = StartCircleBusy;
			CircleBusy.transform.position = CircleBusy.StartPosition;
			IsBusy = true;
		}
		else
		{
			CircleBusy = null;
			IsBusy = false;

		}
	}


}
