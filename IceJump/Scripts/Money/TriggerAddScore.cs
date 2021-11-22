using UnityEngine;


public class TriggerAddScore : MonoBehaviour
{
	private bool _isAdd = true;
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.GetComponent<Player>() && _isAdd)
		{
			ScoreHanlder.Instance.AddScore();
			_isAdd = false;
		}
	}
	private void OnEnable()
	{
		_isAdd = true;
	}
}
