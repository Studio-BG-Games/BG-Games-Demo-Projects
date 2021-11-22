using DefaultNamespace.Services.Console;
using Infrastructure;
using Infrastructure.Configs;
using Infrastructure.LevelState.States;
using Plugins.DIContainer;
using Plugins.GameStateMachines;
using Plugins.Interfaces;
using UnityEngine;

public class BootStrapGame : MonoBehaviour, ICoroutineRunner
{
    [SerializeField] private ConfigLevelName _levelNameConfig;
    [SerializeField] private ConfigGame _configGame;
    [SerializeField] private Curtain _curtainGame;
    [SerializeField] private ConsoleTMP _console;

    [Header("Локализация")] 
    [SerializeField] private ConfigLocalization _russia;

    private DiBox _diBox = DiBox.MainBox;

    private void Start()
    {
        MakeDontDestroyOnLoad();
        LevelStateMachine levelStateMachine = new LevelStateMachine();
        RegisterDI(levelStateMachine);
        _console.Init();
        levelStateMachine.Enter<InitGameScene>();
    }

    private void MakeDontDestroyOnLoad()
    {
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(_curtainGame);
    }

    private void RegisterDI(LevelStateMachine levelStateMachine)
    {
        _diBox.RegisterSingle(_curtainGame);
        _diBox.RegisterSingle(_levelNameConfig);
        _diBox.RegisterSingle(levelStateMachine);
        _diBox.RegisterSingle<ICoroutineRunner>(this);
        _diBox.RegisterSingle(_russia);
        _diBox.RegisterSingle(_configGame);
        _diBox.RegisterSingle<IConsole>(_console);
        CreateSceneLoader();
    }

    private void CreateSceneLoader()
    {
        var sceneLoader = new SceneLoader();
        _diBox.RegisterSingle(sceneLoader);
        _diBox.InjectSingle(sceneLoader);
    }
}
