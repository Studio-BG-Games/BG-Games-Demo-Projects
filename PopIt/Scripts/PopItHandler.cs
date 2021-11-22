using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PopItHandler : MonoBehaviour
{

	[SerializeField] private Transform _popIt;
	[SerializeField] private float _speedRotation;
	private bool isRot = false;

	public void RotationPopIt()
	{
		YandexSDK.instance.ShowInterstitial();
		if (!isRot)
		{
			
			_popIt.DOLocalRotate(new Vector3(_popIt.rotation.x, _popIt.rotation.y, 180f), _speedRotation);
		}
		else
		{
			_popIt.DOLocalRotate(new Vector3(_popIt.rotation.x, _popIt.rotation.y, 0f), _speedRotation);
		}
		isRot = !isRot;
	}
}
