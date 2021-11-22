using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundToButton : MonoBehaviour
{
	[SerializeField] private TypeSound _typeSound;
	public void PlaySound()
	{
		AudioHandler.Instance.PlaySound(_typeSound);
	}
}
