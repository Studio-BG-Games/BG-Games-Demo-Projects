using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerLevel : MonoBehaviour
{
	private enum TypeTriggerLevel
	{
		create,
		destroy,
		end
	}
	[SerializeField] TypeTriggerLevel _type;
	private bool _isActive = true;
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(_type == TypeTriggerLevel.create)
		{
			if (collision.GetComponent<Player>() && _isActive)
			{
				GameHandler.Instance.CreateLevel();
				_isActive = false;
			}
		}
		else if (_type == TypeTriggerLevel.destroy)
		{
			if (collision.GetComponent<Player>() && _isActive)
			{
				GameHandler.Instance.DestroyLevel();
				_isActive = false;
			}
		}
		else if (_type == TypeTriggerLevel.end)
		{
			if (collision.GetComponent<Player>() && _isActive)
			{
				PausedHandler.Instance.EndLevel();
				PausedHandler.Instance.ResetGame();
				Time.timeScale = 0f;
				_isActive = false;
			}
		}
	}
	private void OnEnable()
	{
		_isActive = true;
	}
}
