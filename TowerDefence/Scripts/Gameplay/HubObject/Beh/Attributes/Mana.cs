using System;
using UnityEngine;

namespace Gameplay.HubObject.Beh.Attributes
{
    public class Mana :MonoBehaviour
    {
        [SerializeField] private int _current;
        [SerializeField] private int _maxBase;
        
        private int _realMax;

        public event Action Damaged;
        public event Action Recovered;
        public event Action<float> NewValue;

        public bool TryGet(int value)
        {
            if (value < 0)
            {
                Debug.LogWarning("Запращивается отрицательное колличество манны");
                return false;
            }

            if (_current - value < 0)
                return false;

            _current -= value;
            Clamp();
            Damaged?.Invoke();
            return true;
        }
        
        private void Clamp()
        {
            _current = Mathf.Clamp(_current, 0, _realMax);
            NewValue?.Invoke((float)_current/_realMax);            
        }

        public void Recovery(int value)
        {
            _current += value;
            Recovered?.Invoke();
            Clamp();
        }
    }
}