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
	buttonInteract,
	popup
}
public class AudioHandler : MonoBehaviour
{
	public static AudioHandler Instance;

	[SerializeField] private AudioSource _soundSource;
	[SerializeField] private AudioSource _musicSource;

	public AudioSource SoundSource { get { return _soundSource; } }
	public AudioSource MusicSource { get{ return _musicSource; } }
	[SerializeField] private AudioClip[] _sounds;
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

	public void PlaySound(TypeSound sound)
	{
		_soundSource.PlayOneShot(_sounds[(int)sound]);
	}
}
