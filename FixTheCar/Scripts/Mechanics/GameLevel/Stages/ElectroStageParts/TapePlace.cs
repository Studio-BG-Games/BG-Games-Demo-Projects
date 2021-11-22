using System;
using Mechanics.GameLevel.Stages.ElectroStageParts.Wire;
using UnityEngine;
using UnityEngine.UI;

namespace Mechanics.GameLevel.Stages.ElectroStageParts
{
    [RequireComponent(typeof(Collider2D))]
    public class TapePlace : MonoBehaviour
    {
        public event Action Fixed;
        public bool IsFixed => _currentState != State.Broken; 

        [SerializeField] private Image _image;
        [SerializeField] private WiresPart _wires;
        
        private State _currentState = State.Unactive;
        private bool _canFixed;

        private void Awake()
        {
            ChangeActiveImage(false);
            _canFixed = false;
            _wires.Broked += OnBrokenWires;
            _wires.Fixed += OnFixedWire;
        }

        private void OnDestroy()
        {
            _wires.Broked -= OnBrokenWires;
            _wires.Fixed -= OnFixedWire;
        }

        public void ApplayTape(Tape tape)
        {
            if(_currentState!=State.Broken || !_canFixed)
                return;
            _currentState = State.Fixed;
            ChangeActiveImage(true);
            _canFixed = false;
            Fixed?.Invoke();
        }

        private void OnFixedWire() => _canFixed = true;

        private void OnBrokenWires() => _currentState = State.Broken;
        
        private void ChangeActiveImage(bool isActive)
        {
            var color = _image.color;
            color.a = isActive?1:0;
            _image.color = color;
        }

        private enum State
        {
            Unactive, Broken, Fixed
        }
    }
}