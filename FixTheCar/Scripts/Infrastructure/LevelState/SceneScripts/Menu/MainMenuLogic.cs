using DG.Tweening;
using Factories;
using Infrastructure.Configs;
using Infrastructure.LevelState.States;
using Mechanics.Prompters;
using Plugins.DIContainer;
using Plugins.GameStateMachines;
using Plugins.Interfaces;
using Services.Interfaces;
using UnityEngine;

namespace Infrastructure.LevelState.SceneScripts.Menu
{
    public class MainMenuLogic : MonoBehaviour
    {
        [SerializeField] private FactoryPrompter _factoryPrompter;
        [SerializeField] private float _durationTransit = 2;
        [Min(3)] [SerializeField] private float _zoomCamera;
        [Min(0.1f)][SerializeField] private float _durationCamera;

        [DI] private LevelStateMachine _levelStateMachine;
        [DI] private ConfigLocalization _configLocalization;
        [DI] private IInput _input;
        [DI] private Curtain _curtain;

        private Prompter _currentPropter => _factoryPrompter.Current;
        private Tween _tweenCamera;

        private void Awake()
        {
            _curtain.Unfaded += OnUnfaded;
            _factoryPrompter.ChangeAt(FactoryPrompter.Type.WTF);
        }

        private void OnUnfaded()
        {
            _curtain.Unfaded -= OnUnfaded;
            _currentPropter.Unhide(() =>
            {
                _currentPropter.Say(_configLocalization.Wellcome, ()=> _input.AnyInput += OnAnyInput);
            });
        }

        private void OnAnyInput()
        {
            _tweenCamera = Camera.main.DOOrthoSize(_zoomCamera, _durationCamera);
            _input.AnyInput -= OnAnyInput;
            _currentPropter.Hide();
            _curtain.Fade(() => _levelStateMachine.Enter<GarageScene>(), _durationTransit);
        }

        private void OnDestroy()
        {
            _input.AnyInput -= OnAnyInput;
            _tweenCamera.Kill();
        }
    }
}