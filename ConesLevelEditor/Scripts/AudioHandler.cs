using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum TypeSound : int
{
	button,
	win,
	move,
	failMove,
	moveTo,
	buttonInteract
}
public class AudioHandler : MonoBehaviour
{
	public static AudioHandler Instance;

	[SerializeField] private AudioSource _soundSource;
	[SerializeField] private AudioSource _musicSource;

	[SerializeField] private AudioClip[] _sounds;
	private void Awake()
	{
		if(Instance == null)
		{
			Instance = this;
		}
		DontDestroyOnLoad(gameObject);
	}

	public void PlaySound(TypeSound sound)
	{
		_soundSource.PlayOneShot(_sounds[(int)sound]);
	}
}
