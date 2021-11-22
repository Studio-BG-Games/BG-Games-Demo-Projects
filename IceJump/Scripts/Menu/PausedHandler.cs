using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PausedHandler : MonoBehaviour
{
	public static PausedHandler Instance;
	private const string _sceneGame = "_Game";
	private const string _sceneMenu = "_Menu";

	[SerializeField] private GameObject _pausedPanel;
	[SerializeField] private GameObject _endPanel;

	private void Awake()
	{
		Instance = this;
	}
	public void ResetGame()
	{
		Time.timeScale = 1f;
		GameHandler.Instance.Restart();
		ScoreHanlder.Instance.Restart();
		GameHandler.Instance.StartGame();

	}

	public void ExitGame()
	{
		SceneManager.LoadScene(_sceneMenu);
		Time.timeScale = 1f;
	}

	public void PusedGame(bool active = false)
	{
		_pausedPanel.SetActive(active);
		Time.timeScale = Convert.ToInt32(!active);
	}

	public void EndLevel()
	{
		_endPanel.SetActive(true);
	}
}
