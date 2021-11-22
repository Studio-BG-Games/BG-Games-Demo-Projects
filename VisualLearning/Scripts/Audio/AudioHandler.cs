using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class AudioHandler : MonoBehaviour
{
	[SerializeField] private AudioSource _soundSource;
	[SerializeField] private AudioSource _musicSource;

	private AudioClip[] _sounds;

	[Inject]
	public void Init(AudioClip[] sounds)
	{
		_sounds = sounds;
	}
	public void PlaySound(AudioClip clip)
	{
		_soundSource.PlayOneShot(clip);
	}
}
