using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraHandler : MonoBehaviour
{
	[SerializeField] private GameObject LockMoveToObject;
	[SerializeField] private float _speedMove = 0.2f;

	private void LateUpdate()
	{
		transform.DOMove(new Vector3(0, LockMoveToObject.transform.position.y, -10f), _speedMove);
	}
}
