using System;
using Factories;
using Infrastructure.Configs;
using Mechanics.Prompters;
using Mechanics.Prompters.Interfaces;
using Plugins.DIContainer;
using Services.Interfaces;
using UnityEngine;

namespace Mechanics.GameLevel.Stages.NumbetStageParts.StateMachine
{
    public class GreetingState : NumberStageState
    {
        [SerializeField] private MathPatternState _mathPatternState;
        [SerializeField] private Engine _engine;
        [SerializeField] private AbsDelayPrometed _delayFirst;
        [SerializeField] private AbsDelayPrometed _delaySecond;
        
        [DI] private IInput _input;
        [DI] private FactoryPrompter _factoryPrompter;
        [DI] private ConfigLocalization _configLocalization;

        private Prompter CurrentPromter => _factoryPrompter.Current;
        private bool _canTransit = false;
        
        public override void On()
        {
            _factoryPrompter.ChangeAt(FactoryPrompter.Type.Hello);
            CurrentPromter.Unhide(
                ()=>CurrentPromter.Say(
                    _configLocalization.HelloNumberStage, 
                    ()=>_delayFirst.Activated(OnFirstClick))
                );
        }

        public override void Off() => _canTransit = false;

        public override State TransitToOrNull() => _canTransit ? _mathPatternState : null;

        private void OnFirstClick()
        {
            _input.AnyInput -= OnFirstClick;
            CurrentPromter.Say(_configLocalization.HelloNumberStage2, ()=>_delaySecond.Activated(OnSecondClick));
        }

        private void OnSecondClick()
        {
            _input.AnyInput -= OnSecondClick;
            _engine.GenerateShadow();
            _canTransit = true;
        }
    }
}