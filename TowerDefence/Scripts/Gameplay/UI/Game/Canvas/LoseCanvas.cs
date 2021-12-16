using Infrastructure.SceneStates;
using Plugins.DIContainer;
using Plugins.GameStateMachines;
using Plugins.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.UI.Game.Canvas
{
    public class LoseCanvas : CustomCanvas
    {
        [SerializeField] private Button _button;
        [SerializeField] private float _durationFade = 1;
        
        [DI] private AppStateMachine _appState;
        [DI] private ICurtain _curtain;

        private void Awake()
        {
            _button.onClick.AddListener(EnterTo);
        }

        private void EnterTo()
        {
            _curtain.Fade(()=>_appState.Enter<MainMenu>(), _durationFade);
        }
    }
}