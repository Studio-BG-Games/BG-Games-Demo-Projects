using System;
using UnityEngine;
public class SwitchButton : MonoBehaviour
{
	[SerializeField] private TypeButtonSwitch _typeButtonSwitch;
	public Action<bool> OnSwitchPicture;

	public void SwitchPicture()
	{
		OnSwitchPicture?.Invoke(Convert.ToBoolean((int)_typeButtonSwitch));
	}
}
