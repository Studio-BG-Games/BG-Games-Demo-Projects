using UnityEngine;
using DG.Tweening;
public class CartHandler : MonoBehaviour
{
	[SerializeField] private float[] _positionMove;
	[SerializeField] private float _speedMove;
	[SerializeField] private TriggerToCart[] _triggers;
	[SerializeField] private int _vectorStart;

	private bool _isVector;

	private void Start()
	{
		foreach (var trigger in _triggers)
		{
			trigger.OnTriggerAction += Move;
		}
	}


	private void Update()
	{
		if (_isVector)
		{
			transform.Translate(Vector3.right * _speedMove * Time.deltaTime);
		}
		else
		{
			transform.Translate(Vector3.left * _speedMove * Time.deltaTime);
		}
	}

	public void Move(int vector)
	{
		
		if(vector == 0)
		{
			_isVector = true;
		}
		else if(vector == 1)
		{

			_isVector = false;
		}
	}
}
