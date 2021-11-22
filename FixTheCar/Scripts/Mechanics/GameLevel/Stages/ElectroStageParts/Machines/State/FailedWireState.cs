using Factories;
using Infrastructure.Configs;
using Mechanics.Prompters.Interfaces;
using Plugins.DIContainer;
using Services.Interfaces;
using UnityEngine;

namespace Mechanics.GameLevel.Stages.ElectroStageParts.Machines.State
{
    internal class FailedWireState : ElectorState
    {
        [SerializeField] private SetWiresState _setWiresState;
        [SerializeField] private AbsDelayPrometed _delay;
        
        [DI] private IInput _input;
        [DI] private ConfigLocalization _configLocalization;
        [DI] private FactoryPrompter _factoryPrompter;

        private bool _transit;
        
        public override void On()
        {
            _factoryPrompter.ChangeAt(FactoryPrompter.Type.DontKnow);
            _factoryPrompter.Current.Say(_configLocalization.FailSetWires, ()=>_delay.Activated(OnInput));
        }

        public override void Off() => _transit = false;

        public override Stages.State TransitToOrNull() => _transit ? _setWiresState : null;

        private void OnInput()
        {
            _input.AnyInput -= OnInput;
            _transit = true;
        }
    }
}