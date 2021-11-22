using Mechanics.GameLevel.Stages.СanistorStageParts.StateMAchine;
using UnityEngine;

namespace Mechanics.GameLevel.Stages.СanistorStageParts.States
{
    public class CanistroActionCanistroStageState : CanistroStageState
    {
        [SerializeField] private BigCanistro _bigCanistro;
        [SerializeField] private CanistroAgregator _canistroAgregator;

        [SerializeField] private ChoiseCanistroState _canistroChoiseState;
        [SerializeField] private EndingCanistroStageState _ending;

        private CanistroStageState _stateForReturn;
        
        public override void On()
        {
            _canistroAgregator.SomeCanistroActionFinish += OnFinishCanistroState;
            _bigCanistro.Finished += OnCanistroFinished;
        }

        public override void Off()
        {
            _canistroAgregator.SomeCanistroActionFinish -= OnFinishCanistroState;
            _bigCanistro.Finished -= OnCanistroFinished;
            _stateForReturn = null;
        }

        private void OnCanistroFinished() => _stateForReturn = _ending;

        public override State TransitToOrNull() => _stateForReturn;

        private void OnFinishCanistroState() => _stateForReturn = _canistroChoiseState;
    }
}