using Factories;
using Mechanics.GameLevel.Stages.СanistorStageParts.StateMAchine;
using UnityEngine;

namespace Mechanics.GameLevel.Stages.СanistorStageParts.States
{
    public class ChoiseCanistroState : CanistroStageState
    {
        [SerializeField] private CanistroAgregator _canistroAgregator;
        [SerializeField] private CanistroActionCanistroStageState _transitState;

        private CanistroStageState _stageForReturn;
        
        public override void On()
        {
            FactoryPrompter.Current.Unhide(
                ()=>
                {
                    FactoryPrompter.ChangeAt(FactoryPrompter.Type.Hello).Say(ConfigLocalization.ChooseAnyCanisters);
                    _canistroAgregator.SomeCanistroActionStart += OnStartActionCanistro;
                    _canistroAgregator.ChangeInteractTo(true);
                    
                });
        }

        public override void Off()
        {
            _canistroAgregator.SomeCanistroActionStart -= OnStartActionCanistro;
            _canistroAgregator.ChangeInteractTo(false);
            _stageForReturn = null;
        }

        public override State TransitToOrNull() => _stageForReturn;

        private void OnStartActionCanistro() => _stageForReturn = _transitState;
    }
}