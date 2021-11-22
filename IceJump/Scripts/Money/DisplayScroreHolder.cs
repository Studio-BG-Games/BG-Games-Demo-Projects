using UnityEngine;
using UnityEngine.UI;

public class DisplayScroreHolder : MonoBehaviour
{
	[SerializeField] private Text _textScore;
	[SerializeField] private Text _textScoreCurrent;

	[SerializeField] private Text _textCountDiamond;
	[SerializeField] private Text _textCountDiamondCurrent;

	[SerializeField] private Text _textScoreOld;

	private void Start()
	{
		ScoreHanlder.Instance.OnScroreUpdateAction += UpdateDisplayScore;
		ScoreHanlder.Instance.OnDiamondUpdateAction += UpdateDisplayDiamond;
	}

	private void UpdateDisplayScore(int score, int scoreOld)
	{
		_textScore.text = score.ToString();
		_textScoreCurrent.text = score.ToString();

		_textScoreOld.text = scoreOld.ToString();

	}

	private void UpdateDisplayDiamond(int count, int countOld)
	{
		_textCountDiamond.text = count.ToString();
		_textCountDiamondCurrent.text = count.ToString();
	}
}
