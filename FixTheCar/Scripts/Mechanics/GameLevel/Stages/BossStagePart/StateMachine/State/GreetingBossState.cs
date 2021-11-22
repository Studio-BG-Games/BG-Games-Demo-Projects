using System;
using Factories;
using Infrastructure.Configs;
using Mechanics.Prompters.Interfaces;
using Plugins.DIContainer;
using Services.Interfaces;
using UnityEngine;

namespace Mechanics.GameLevel.Stages.BossStagePart.StateMachine.State
{
    public class GreetingBossState : StageBossState
    {
        [SerializeField] private FightWithBoss _fightWithBoss;
        [SerializeField] private AbsDelayPrometed _delay;
        
        [DI] private FactoryPrompter _factoryPrompter;
        [DI] private ConfigLocalization _configLocalization;
        [DI] private IInput _input;
        private bool _isTransit = false;

        public override void On()
        {
            _factoryPrompter.ChangeAt(FactoryPrompter.Type.Hello);
            _factoryPrompter.Current.Unhide(
                ()=>_factoryPrompter.Current.Say(_configLocalization.HelloBossStage, 
                    ()=>_delay.Activated(OnAnyInput)));
        }

        private void OnAnyInput()
        {
            _input.AnyInput -= OnAnyInput;
            _factoryPrompter.Current.Hide(()=>_isTransit=true);
        }

        public override void Off() => _isTransit = false;

        public override Stages.State TransitToOrNull() => _isTransit ? _fightWithBoss : null;
    }
}