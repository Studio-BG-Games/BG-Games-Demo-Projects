using System;
using System.Collections.Generic;
using System.Linq;
using Factorys;
using Gameplay.Builds;
using Gameplay.Builds.Data;
using Gameplay.Builds.Data.Marks;
using Gameplay.GameSceneScript;
using Gameplay.StateMachine.GameScene;
using Infrastructure.SceneStates;
using Plugins.DIContainer;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.UI.Game.Canvas
{
    public class CanvasButtonChoise : CustomCanvas
    {
        [SerializeField] private ButtonTrigger _mainBuildButton;
        [SerializeField] private ButtonTrigger _extraBuildButton;
        [SerializeField] private ButtonTrigger _unitsButton;
        [SerializeField] private Button _buttleButton;
        [SerializeField] private Button _SettingButton;

        public ButtonTrigger MainBuildButton => _mainBuildButton;
        public ButtonTrigger ExtraBuildButton => _extraBuildButton;
        public ButtonTrigger UnitsButton => _unitsButton;

        [DI] private Construct _construct;
        [DI] private ControlBuildOnMap _controlBuildOnMap;
        [DI] private GameSceneStateMachine _gameSceneStateMachine;

        private void Awake()
        {
            _extraBuildButton.ActionOnEnable += ActivateGridByExtra;
            _extraBuildButton.ActionOnDisable += DeactivateGridByExtra;
            _extraBuildButton.Click += OnClickExtra;
            
            _mainBuildButton.ActionOnEnable += ActivateGridByMain;
            _mainBuildButton.ActionOnDisable += DeactivateGridByMain;
            _mainBuildButton.Click += OnClickMain;

            _unitsButton.ActionOnEnable += ActivateGridByUnit;
            _unitsButton.ActionOnDisable += DeactivateGridByUnit;
            _unitsButton.Click += OnClickUnit;

            _buttleButton.onClick.AddListener( OnCLickBattleButton);
        }

        private void OnEnable()
        {
            _controlBuildOnMap.On();
        }

        private void OnDisable()
        {
            _controlBuildOnMap.Off();
            ChangeStateButton(new []{_extraBuildButton, _mainBuildButton, _unitsButton}, false);
        }

        private void OnClickUnit()
        {
            ChangeStateButton(new[] {_extraBuildButton, _mainBuildButton}, false);
        }

        private void DeactivateGridByUnit()
        {
            _controlBuildOnMap.On();
            _construct.Off();
        }

        private void ActivateGridByUnit()
        {
            _controlBuildOnMap.Off();
            _construct.On<ConstructUnitState>();
        }

        private void OnCLickBattleButton() => _gameSceneStateMachine.Enter<FightState>();


        private void OnClickExtra() => ChangeStateButton(new []{_mainBuildButton, _unitsButton}, false);

        private void ActivateGridByExtra()
        {
            _controlBuildOnMap.Off();
            _construct.On<ConstructExtraBuildState>();
        }

        private void DeactivateGridByExtra()
        {
            _construct.Off();
            _controlBuildOnMap.On();
        }

        private void DeactivateGridByMain()
        {
            _construct.Off();
            _controlBuildOnMap.On();
        }

        private void ActivateGridByMain()
        {
            _controlBuildOnMap.Off();
            _construct.On<ConstructMainBuildState>();
        }

        private void OnClickMain() => ChangeStateButton(new []{_extraBuildButton, _unitsButton}, false);

        private void ChangeStateButton(ButtonTrigger[] button, bool to)
        {
            foreach (var buttonTrigger in button) 
                if(buttonTrigger.IsEnable)
                    buttonTrigger.ForcedChangeState(to, true);
        }
    }
}