using System;
using UnityEngine;

namespace Mechanics.Garage
{
    public class Lamp : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteLight;
        
        private void Awake() => Off();

        public void On() => _spriteLight.enabled = true;

        public void Off() => _spriteLight.enabled = false;
    }
}