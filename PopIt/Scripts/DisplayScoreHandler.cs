using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayScoreHandler : MonoBehaviour
{
	public static DisplayScoreHandler Instance;
	[SerializeField] private Text _textScore;
	[SerializeField] private Text _textLevel;

	private int _score;
	private int _level;

	private void Awake()
	{
		Instance = this;
		_level = 1;
	}

	public void UpdateTextScore()
	{
		_score++;
		if(_score < 10)
		{
			_textScore.text = "00" + _score.ToString();
		}
		else if (_score < 100)
		{
			_textScore.text = "0" + _score.ToString();
		}
		else
		{
			_textScore.text = _score.ToString();
		}
	
	}

	public void UpdateTextLevel()
	{
		_level++;
		if (_level < 10)
		{
			_textLevel.text = "0" + _level.ToString();
		}
		else
		{
			_textLevel.text = _level.ToString();
		}

	}
}
