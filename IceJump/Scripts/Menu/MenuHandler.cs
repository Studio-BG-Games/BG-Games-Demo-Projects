using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuHandler : MonoBehaviour
{
	private const string _sceneGame = "_Game";

	public void StartGame()
	{
		SceneManager.LoadScene(_sceneGame);
	}
}
