using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circle : MonoBehaviour
{
	public Vector3 StartPosition;
	[SerializeField] private int _id;
	public bool isOut = false;
	public int Id{ get{ return _id; } }

	public void SetPosition(Vector3 position)
	{
		transform.position = position;
	}

	public void SetStartPosition()
	{
		StartPosition = transform.position;
	}
}
