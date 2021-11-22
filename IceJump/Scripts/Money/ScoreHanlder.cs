using System;
using UnityEngine;

public class ScoreHanlder : MonoBehaviour
{
	public static ScoreHanlder Instance;
	public Action<int, int> OnScroreUpdateAction;
	public Action<int, int> OnDiamondUpdateAction;

	private int _score;
	private int _diamondCount;
	private int _scoreOld;
	private int _diamondOld;
	private void Awake()
	{
		_score = default;
		_diamondCount = default;
		if (PlayerPrefs.HasKey("scoreOld"))
		{
			_scoreOld = PlayerPrefs.GetInt("scoreOld");
		}
		else
		{
			_scoreOld = default;
		}

		if (PlayerPrefs.HasKey("diamondOld"))
		{
			_diamondOld = PlayerPrefs.GetInt("diamondOld");
		}
		else
		{
			_diamondOld = default;
		}

		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			Destroy(gameObject);
		}

	}
	public void AddScore()
	{
		_score++;
		if(_score > PlayerPrefs.GetInt("scoreOld"))
		{
			PlayerPrefs.SetInt("scoreOld", _score);
			_scoreOld = PlayerPrefs.GetInt("scoreOld");
		}
		OnScroreUpdateAction?.Invoke(_score, _scoreOld);
	}

	public void AddDiamond()
	{
		_diamondCount++;
		if (_diamondCount > PlayerPrefs.GetInt("diamondOld"))
		{
			PlayerPrefs.SetInt("diamondOld", _diamondCount);
			_diamondOld = PlayerPrefs.GetInt("diamondOld");
		}
		OnDiamondUpdateAction?.Invoke(_diamondCount, _diamondOld);
	}

	public void Restart()
	{

		OnScroreUpdateAction?.Invoke(_score, _scoreOld);
		OnDiamondUpdateAction?.Invoke(_diamondCount, _diamondOld);
		_score = default;
		_diamondCount = default;
	}
}
