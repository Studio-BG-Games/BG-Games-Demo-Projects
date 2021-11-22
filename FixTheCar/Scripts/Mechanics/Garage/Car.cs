using System;
using Infrastructure.Configs;
using JetBrains.Annotations;
using UnityEngine;

namespace Mechanics.Garage
{
    [RequireComponent(typeof(Collider2D))]
    public class Car : MonoBehaviour
    {
        public ConfigLevel LevelConfigCar { get; private set; }
        public event Action Selected;
        public event Action Unselected;

        private State _currentState;
        
        public void Init(ConfigLevel configLevel)
        {
            LevelConfigCar = configLevel;
            _currentState = State.Unselected;
        }

        [CanBeNull]
        public Car Select()
        {
            if (_currentState == State.Unselected)
            {
                _currentState = State.Selected;
                Selected?.Invoke();
                return this;
            }
            return null;
        }

        public void Unselect()
        {
            if (_currentState == State.Selected)
            {
                _currentState = State.Unselected;
                Unselected?.Invoke();
            }
        }
        
        private enum State
        {
            Selected, Unselected
        }
    }
}