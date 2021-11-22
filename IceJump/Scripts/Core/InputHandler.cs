using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
	public void Jump()
	{
		Player.Instance.Jump();
	}
}
