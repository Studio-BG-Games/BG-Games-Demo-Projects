using Factories;
using Infrastructure.Configs;
using Plugins.DIContainer;
using UnityEngine;

namespace Mechanics.GameLevel.Stages.ElectroStageParts.Machines.State
{
    public class FailElectro : ElectorState
    {
        [SerializeField] private TapeAgregator _tapeAgregator;
        [SerializeField] private SetTapeState _setTapeState;

        [DI] private ConfigLocalization _configLocalization;
        [DI] private FactoryPrompter _factoryPrompter;

        private ElectorState _stateToChange = null;
        
        public override void On()
        {
            if (_tapeAgregator.IsAllTapeFixed())
                ChangeState();
            else
                EnterToState();
        }

        public override void Off() => _stateToChange = null;

        public override Stages.State TransitToOrNull() => _stateToChange;

        private void EnterToState()
        {
            _tapeAgregator.SomeTapeFixed += OnSomeTapedFixed;
            _factoryPrompter.ChangeAt(FactoryPrompter.Type.WTF);
            _factoryPrompter.Current.Say(_configLocalization.FailElectroMove);
        }

        private void OnSomeTapedFixed()
        {
            _tapeAgregator.SomeTapeFixed -= OnSomeTapedFixed;
            ChangeState();
        }

        private void ChangeState() => _stateToChange = _setTapeState;
    }
}