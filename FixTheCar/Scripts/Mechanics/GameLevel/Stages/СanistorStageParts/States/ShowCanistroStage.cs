using System.Collections;
using Mechanics.GameLevel.Stages.СanistorStageParts.StateMAchine;
using UnityEngine;

namespace Mechanics.GameLevel.Stages.СanistorStageParts.States
{
    public class ShowCanistroStage : CanistroStageState
    {
        [SerializeField] private CanistroAgregator _canistroAgregator;
        [Min(0)][SerializeField] private float _durationUnfade;
        [Min(0)][SerializeField] private float _durationShow;
        [Min(0)][SerializeField] private float _durationFade;

        [SerializeField] private ChoiseCanistroState _stateToTransit;
        private bool _isTransit;
        
        public override void On()
        {
            FactoryPrompter.Current.Hide();
            _canistroAgregator.Show(_durationUnfade, () => StartCoroutine(WaitAndHideCanistro()));
        }

        public override void Off() => _isTransit = false;

        public override State TransitToOrNull() => _isTransit ? _stateToTransit : null;

        private IEnumerator WaitAndHideCanistro()
        {
            yield return new WaitForSeconds(_durationShow);
            _canistroAgregator.Hide(_durationFade, ()=>_isTransit=true);
        }
    }
}