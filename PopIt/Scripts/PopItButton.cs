using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopItButton : MonoBehaviour
{
	public int Id;
	private bool _isPop = false;

	private GameHandler _gameHandler;
	private DisplayScoreHandler _displayScoreHandler;

	[SerializeField] private bool _isAd;

	private void Start()
	{
		_gameHandler = GameHandler.Instance;
		_displayScoreHandler = DisplayScoreHandler.Instance;
	}

	private void OnMouseEnter()
	{
		if (InputHandler.Instance.IsDown)
		{
			ButtonDown();
		}
		
	}

	private void OnMouseDown()
	{
		if (_isAd)
		{
			YandexSDK.instance.ShowInterstitial();
		}
		ButtonDown();
	}

	private void ButtonDown()
	{

		_gameHandler.BallDestroy(Id);
		_displayScoreHandler.UpdateTextScore();
		if (_isPop)
		{
			AudioHandler.Instance.PlaySound(TypeSounds.popOne);
			_isPop = false;
		}
		else
		{
			AudioHandler.Instance.PlaySound(TypeSounds.popTwo);
			_isPop = true;
		}

		transform.rotation = new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z + 180, 1f);
	}
}
