using System;
using Infrastructure.LevelState.States;
using Plugins.DIContainer;
using Plugins.GameStateMachines;
using Plugins.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.Infrastructure.LevelState.SceneScripts.SketchBookScene
{
    public class SketckbookChangeStateGame : MonoBehaviour
    {
        [SerializeField] private Button _exitButton;
        
        [DI] private LevelStateMachine _stateMachine;
        [DI] private Curtain _curtain;

        private void Awake() => _exitButton.onClick.AddListener(ExitGame);

        private void ExitGame() => _curtain.Fade(()=>_stateMachine.Enter<MainMenu>());
    }
}