using UnityEngine;

namespace Mechanics.GameLevel.Stages
{
    public abstract class StageStateMachine<T> : MonoBehaviour where T : State
    {
        [SerializeField] private T _startState;

        private T _currentState;
        
        private void Awake() => enabled = false;

        public void StartMe()
        {
            if(enabled)
                return;
            _currentState = _startState;
            _currentState.On();
            enabled = true;
        }

        private void Update()
        {
            _currentState.Step();
            CahngeState(_currentState.TransitToOrNull() as T);
        }

        private void CahngeState(T transitToOrNull)
        {
            if(!transitToOrNull)
                return;
            _currentState.Off();
            _currentState = transitToOrNull;
            _currentState.On();
        }
    }
}