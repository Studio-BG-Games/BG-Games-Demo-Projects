using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
	public bool IsGround;
	private void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.CompareTag("Cart"))
		{
			IsGround = true;
		}
		
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		IsGround = false;
	}
}
