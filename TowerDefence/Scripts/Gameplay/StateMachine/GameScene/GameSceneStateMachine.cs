using System;
using System.Collections;
using System.Collections.Generic;
using Plugins.DIContainer;
using Plugins.Interfaces;
using UnityEngine;

namespace Gameplay.StateMachine.GameScene
{
    public class GameSceneStateMachine
    {
        private Dictionary<Type, GameSceneState> _dictionaryStates = new Dictionary<Type, GameSceneState>();
        private GameSceneState _currentState;

        [DI] private ICoroutineRunner _coroutineRunner;
        private Coroutine _coroutineUpdateState;

        public Type CurrentState => _currentState.GetType();

        public event Action<Type> EnteredTo;
        
        public void Enter<T>() where T : GameSceneState, new()
        {
            GameSceneState enteredState = null;
            _dictionaryStates.TryGetValue(typeof(T), out enteredState);
            if (enteredState == null)
            {
                enteredState = new T();
                DiBox.MainBox.InjectSingle(enteredState);
                _dictionaryStates.Add(typeof(T),enteredState);
            }

            if(_coroutineUpdateState!=null)
                _coroutineRunner.StopCoroutine(_coroutineUpdateState);
            
            _currentState?.Exit();
            _currentState = enteredState;
            _currentState.Enter();

            _coroutineUpdateState =  _coroutineRunner.StartCoroutine(StartUpdateState(_currentState));
            EnteredTo?.Invoke(CurrentState);
        }

        public void Stop()
        {
            if(_coroutineUpdateState!=null)
                _coroutineRunner.StopCoroutine(_coroutineUpdateState);
            _currentState?.Exit();
        }
        
        private IEnumerator StartUpdateState(GameSceneState currentState)
        {
            while (true)
            {
                currentState.Update();
                yield return null;
            }
        }
    }
}