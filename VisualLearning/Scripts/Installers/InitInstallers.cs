using UnityEngine;
using Zenject;

public class InitInstallers : MonoInstaller
{
    [SerializeField] private Configuration _configuration;
    public override void InstallBindings()
    {
        Container
            .BindInstance(_configuration.SettingsGame)
            .AsSingle();
        Container
            .BindInstance(_configuration.SettingsAudio)
            .AsSingle();

    }
}