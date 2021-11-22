using Factories;
using Plugins.DIContainer;
using UnityEngine;

namespace Mechanics.GameLevel.Stages.NumbetStageParts.StateMachine
{
    public class MathPatternState : NumberStageState
    {
        [SerializeField] private BoxClickHandler _boxClickHandler;
        [SerializeField] private NumberStage _stage;
    
        [Header("Other state")]
        [SerializeField] private SuccesNumberStage _succesNumberStage;
        [SerializeField] private FailNumberStage _failNumberStage;

        [DI] private FactoryPrompter _factoryPrompter;            
        
        private NumberStageState _stageToReturn = null;
        
        public override void On()
        {
            _factoryPrompter.ChangeAndSayNoneAnimated(FactoryPrompter.Type.Idea, _stage.CurrentPatternString());
            _boxClickHandler.SuccessClick += OnSuccessClick;
            _boxClickHandler.FailClick += OnFailClick;
        }

        public override void Off()
        {
            _stageToReturn = null;
            _boxClickHandler.SuccessClick -= OnSuccessClick;
            _boxClickHandler.FailClick -= OnFailClick;
        }

        public override State TransitToOrNull() => _stageToReturn;

        private void OnSuccessClick() => _stageToReturn = _succesNumberStage;

        private void OnFailClick() => _stageToReturn = _failNumberStage;
    }
}