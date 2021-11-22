using System.Collections;
using Factories;
using Infrastructure.Configs;
using Plugins.DIContainer;
using UnityEngine;

namespace Mechanics.GameLevel.Stages.NumbetStageParts.StateMachine
{
    public class SuccesNumberStage : NumberStageState
    {
        [SerializeField] private MathPatternState _mathPatternState;
        [SerializeField] private EndNumberStage _endNumberStage;
        [SerializeField] private Engine _engine;
        
        [DI] private FactoryPrompter _factoryPrompter;
        [DI] private ConfigLocalization _configLocalization;

        private NumberStageState _stateToTransit;
        
        public override void On()
        {
            _factoryPrompter.ChangeAndSayNoneAnimated(FactoryPrompter.Type.Idea, _configLocalization.PraiseNumber);
            _engine.NewStage += OnNewStage;
            _engine.Completed += OnComleted;
        }

        public override void Off()
        {
            _engine.Completed -= OnComleted;
            _engine.NewStage -= OnNewStage;
            _stateToTransit = null;
        }

        public override State TransitToOrNull() => _stateToTransit;

        private void OnComleted() => _stateToTransit = _endNumberStage;
        
        private void OnNewStage() => StartCoroutine(ChangeState());

        private IEnumerator ChangeState()
        {
            yield return null;
            _stateToTransit = _mathPatternState;
        }
    }
}