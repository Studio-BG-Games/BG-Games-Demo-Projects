using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using DefaultNamespace.Ad.Buttons;
using DefaultNamespace.Infrastructure.Data;
using Gameplay;
using Gameplay.Localizators;
using Gameplay.Map.ConfigData;
using Gameplay.Map.Generator;
using GoogleMobileAds.Api;
using Infrastructure.ConfigData;
using Infrastructure.SceneStates;
using Interface;
using PerlinNode;
using Plugins.DIContainer;
using Plugins.GameStateMachines;
using Plugins.Interfaces;
using Plugins.Sound;
using Services;
using Services.Inputs;
using UnityEngine;
using UnityEngine.Audio;

public class BootStrapGame : MonoBehaviour, ICoroutineRunner
{
    [SerializeField] private ConfigGame _configGame;
    [SerializeField] private CurtainProgress _curtainProgress;
    [SerializeField] private Curtain _curtain;
    [SerializeField] private AudioSource _audioSourceTemplate;
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private Localizator _localizator;
    [SerializeField] private NotificationByFireBase _notificationByFireBase;

    private DiBox _diBox = DiBox.MainBox;
    private FireBaseProvider _onlineProvider;

    private void Awake()
    {
        MobileAds.Initialize(x=>{});
        _onlineProvider = new FireBaseProvider();
        _onlineProvider.Connected += callback;
        _onlineProvider.Connect();

        void callback()
        {
            _onlineProvider.Connected -= callback;
            _notificationByFireBase.Init();
            BoostrapAfterConnectedToBd(_onlineProvider);
        }
    }


    private void BoostrapAfterConnectedToBd(FireBaseProvider onlineProvider)
    {
        var appStateMachine = new AppStateMachine();
        var localProvider = new BinaryProvider();
        var audioMixerScripts = new AudioMixerScript();

        _localizator.Init(false);

        _diBox.RegisterSingle(new TimeScaler());
        _diBox.RegisterSingle(audioMixerScripts);
        _diBox.RegisterSingle(_audioMixer);
        _diBox.RegisterSingle<ICurtain>(_curtain);
        _diBox.RegisterSingle<SaveDataProvider>(onlineProvider, SaveDataProvider.OnlineID);
        _diBox.RegisterSingle<IActionFireBase>(onlineProvider);
        _diBox.RegisterSingle<SaveDataProvider>(localProvider, SaveDataProvider.LocalID);
        _diBox.RegisterSingle(_configGame);
        _diBox.RegisterSingle<ICoroutineRunner>(this);
        _diBox.RegisterSingle(appStateMachine);
        _diBox.RegisterSingle(_localizator);
        _diBox.RegisterSingle<ICurtainProgress>(_curtainProgress);
        var soundSystem = new SoundSystem(_audioSourceTemplate, 25);
        _diBox.RegisterSingle(soundSystem);
        _diBox.InjectSingle(soundSystem);
        CreateInput();
        _configGame.InitConfig(onlineProvider);
        _diBox.InjectSingle(audioMixerScripts);

        _curtain.Unfade(0);
        _curtainProgress.Unfade(0);

        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(_curtainProgress);
        DontDestroyOnLoad(_curtain);
        appStateMachine.Enter<InitScene>();
    }

    private void CreateInput()
    {
        if(Application.isEditor)
            _diBox.InjectAndRegisterAsSingle<IInput>(new PCInput());
        else
            _diBox.InjectAndRegisterAsSingle<IInput>(new MobileInput());
    }
}