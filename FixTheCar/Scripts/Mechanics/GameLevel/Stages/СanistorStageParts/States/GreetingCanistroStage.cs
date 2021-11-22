using System;
using Factories;
using Mechanics.GameLevel.Stages.СanistorStageParts.StateMAchine;
using Mechanics.Prompters.Interfaces;
using UnityEngine;

namespace Mechanics.GameLevel.Stages.СanistorStageParts.States
{
    public class GreetingCanistroStage : CanistroStageState
    {
        [SerializeField] private ShowCanistroStage _stateToTransit;
        [SerializeField] private AbsDelayPrometed _delayFirst;
        [SerializeField] private AbsDelayPrometed _delaySecond;
        
        private bool _isTransit;
        
        public override void On()
        {
            FactoryPrompter.ChangeAt(FactoryPrompter.Type.Hello);
            FactoryPrompter.Current.Unhide(
                ()=>FactoryPrompter.Current.Say(ConfigLocalization.HelloCanistorStage, 
                    ()=>_delayFirst.Activated(OnAnyKey)));
            
        }

        private void OnAnyKey()
        {
            InputPlayer.AnyInput -= OnAnyKey;
            FactoryPrompter.Current.Say(ConfigLocalization.HelloCanistorStage2, 
                () => _delaySecond.Activated(OnAnyKey2));
        }

        private void OnAnyKey2()
        {
            InputPlayer.AnyInput -= OnAnyKey2;
            _isTransit = true;
        }

        public override void Off() => _isTransit = false;

        public override State TransitToOrNull() => _isTransit ? _stateToTransit : null;
    }
}