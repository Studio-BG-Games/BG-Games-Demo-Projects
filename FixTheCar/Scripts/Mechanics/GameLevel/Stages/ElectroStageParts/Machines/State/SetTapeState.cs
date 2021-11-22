using Factories;
using Infrastructure.Configs;
using Plugins.DIContainer;
using UnityEngine;

namespace Mechanics.GameLevel.Stages.ElectroStageParts.Machines.State
{
    public class SetTapeState : ElectorState
    {
        [SerializeField] private ElectroPath electroPath;
        [SerializeField] private FailElectro _failElectro;
        [SerializeField] private EndingElectro _endingElectro;
        
        [DI] private ConfigLocalization _configLocalization;
        [DI] private FactoryPrompter _factoryPrompter;

        private ElectorState _electorState;
        
        public override void On()
        {
            _factoryPrompter.Current.Say(_configLocalization.StartTapeStage);
            electroPath.StartMove();
            electroPath.Failed += OnFailed;
            electroPath.Finished += OnFinished;
        }

        public override void Off()
        {
            electroPath.Failed -= OnFailed;
            electroPath.Finished -= OnFinished;
            _electorState = null;
        }

        public override Stages.State TransitToOrNull() => _electorState;
        
        private void OnFinished() => _electorState = _endingElectro;

        private void OnFailed() => _electorState = _failElectro;
    }
}