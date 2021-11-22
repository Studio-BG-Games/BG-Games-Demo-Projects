using Factories;
using Infrastructure.Configs;
using Mechanics.GameLevel.Stages.ElectroStageParts.Wire;
using Plugins.DIContainer;
using UnityEngine;

namespace Mechanics.GameLevel.Stages.ElectroStageParts.Machines.State
{
    public class SetWiresState : ElectorState
    {
        [SerializeField] private FailedWireState _failedWireState;
        [SerializeField] private SucssedWireState _nonefialedWireState;
        [SerializeField] private SetTapeState _tapeState;
        [SerializeField] private InteractWithWirePart _interactWithWire;
        [SerializeField] private Wires _wires;

        [DI] private ConfigLocalization _configLocalization;
        [DI] private FactoryPrompter _factoryPrompter;
        
        private ElectorState _stateToTransit;
        
        public override void On()
        {
            _factoryPrompter.ChangeAt(FactoryPrompter.Type.Hello);
            _factoryPrompter.Current.Say(_configLocalization.TakeAnyWire);
            _interactWithWire.OnInteract();
            _wires.FixedAll += OnFixedAll;
            _wires.FixedOnePart += OnFixedOne;
            _wires.FaildSet += OnFailFixed;
        }
        
        public override void Off()
        {
            _interactWithWire.OffInteract();
            _wires.FixedAll -= OnFixedAll;
            _wires.FixedOnePart -= OnFixedOne;
            _wires.FaildSet -= OnFailFixed;
            _stateToTransit = null;
        }

        public override Stages.State TransitToOrNull() => _stateToTransit;
        
        private void OnFailFixed() => _stateToTransit = _failedWireState;

        private void OnFixedOne() => _stateToTransit = _nonefialedWireState;

        private void OnFixedAll() => _stateToTransit = _tapeState;
    }
}