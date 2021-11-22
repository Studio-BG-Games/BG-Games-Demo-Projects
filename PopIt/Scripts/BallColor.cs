using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallColor : MonoBehaviour, IColorablyBall
{
	public int Id;
	public Vector2 StartPosition;

	private void Start()
	{
		SaveStartPosition();
	}
	public void DestroyBall()
	{
		gameObject.SetActive(false);
	}

	public void ResetBall()
	{
		transform.position = StartPosition;
		gameObject.SetActive(true);
	}

	public void SaveStartPosition()
	{
		StartPosition = transform.position;
	}
}
