using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour
{
	IEnumerator Start()
	{
		yield return new WaitForSeconds(0.02f);
		GameHandler.Instance.StartGame();
	}
}
