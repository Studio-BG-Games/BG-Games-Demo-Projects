using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedSkinButton : MonoBehaviour
{
	public Action OnSelectedSkinAction;
	public void SelectedSkinToPlayer()
	{
		OnSelectedSkinAction?.Invoke();
	}
}
