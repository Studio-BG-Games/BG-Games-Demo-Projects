using UnityEngine;
using System;
using Zenject;

public class AudioInstaller : MonoInstaller
{
    private Settings _settings;

    [Inject]
    public void Init(Settings settings)
	{
        _settings = settings;
	}
    public override void InstallBindings()
    {
        Container
            .Bind<AudioHandler>()
            .FromComponentInNewPrefab(_settings.AudioHandler)
            .AsSingle();
        Container
            .Bind<AudioClip[]>()
            .FromInstance(_settings.Sounds)
            .AsSingle();
    }

    [Serializable]
    public class Settings
	{
        [SerializeField] private AudioClip[] _sounds;
        [SerializeField] private AudioHandler _audioHandler;

        public AudioClip[] Sounds { get{ return _sounds; } }
        public AudioHandler AudioHandler{ get{ return _audioHandler; } }
    }
}