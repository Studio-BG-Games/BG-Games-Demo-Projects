using System;
using Infrastructure.SceneStates;
using Plugins.DIContainer;
using Plugins.GameStateMachines;
using Plugins.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.UI.Game.Canvas
{
    public class WinCanvas : CustomCanvas
    {
        [SerializeField] private Button _button;
        [SerializeField] private float _duration;

        [DI] private AppStateMachine _appState;
        [DI] private ICurtain _curtain;
        [DI] private GameSceneData _gameSceneData;

        private void Awake()
        {
            _button.onClick.AddListener(EnterTo);
        }

        private void EnterTo()
        {
            _curtain.Fade(()=>{_appState.Enter<MainMenu, MainMenu_WinData>(new MainMenu_WinData(_gameSceneData.LevelSetProfile));}, _duration);
        }
    }
}