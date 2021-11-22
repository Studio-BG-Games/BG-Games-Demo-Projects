using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PausedHandler : MonoBehaviour
{
	public static PausedHandler Instance;
	[SerializeField] private GameObject _mapLevels;
	[SerializeField] private GameObject _winPanel;
	[SerializeField] private GameObject _homePanel;
	[SerializeField] private Text _textCurrentCompletLevel;
	public Text _textCountMoleculesReward;
	private const string _sceneMenu = "_Menu";

	private void Awake()
	{
		Instance = this;
	}
	public void ContinueToMenu()
	{
		SceneManager.LoadScene(_sceneMenu);
	}
	public void OpenMapLevels()
	{
		_mapLevels.SetActive(true);
	}
	public void CloseMapLevels()
	{
		_mapLevels.SetActive(false);
	}

	public void OpenWinPanel()
	{
		_winPanel.SetActive(true);
		_textCountMoleculesReward.text = GameHandler.Instance.CurrentLevel.RewardMolecules.ToString();
		_textCurrentCompletLevel.text = $"спнбемэ { GameHandler.Instance.CurrentLevel.Id}\nопнидем";
	}
	public void CloseWinPanel()
	{
		_winPanel.SetActive(false);
	}

	public void CloseHomePanel()
	{
		_homePanel.SetActive(false);
	}
}
