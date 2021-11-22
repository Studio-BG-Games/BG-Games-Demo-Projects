using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypeSounds
{
	popOne,
	popTwo
}
public class AudioHandler : MonoBehaviour
{
	public static AudioHandler Instance;
	[SerializeField] private AudioSource _soundSource;
	[SerializeField] private AudioSource _musicSource;

	[SerializeField] private AudioClip[] _audios;

	private void Awake()
	{
		if(Instance == null)
		{
			Instance = this;
		}
		else
		{
			Destroy(gameObject);
		}

		DontDestroyOnLoad(gameObject);
		
	}

	public void PlaySound(TypeSounds type)
	{
		_soundSource.PlayOneShot(_audios[(int)type]);
	}

}
