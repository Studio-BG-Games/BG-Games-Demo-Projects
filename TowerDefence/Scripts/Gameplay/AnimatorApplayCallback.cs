using System;
using UnityEngine;

namespace Gameplay
{
    [RequireComponent(typeof(Animator))]
    public class AnimatorApplayCallback : MonoBehaviour
    {
        private Animator _animator;
        public event Action<string> CallbackAnimation;

        private void Awake() => _animator = GetComponent < Animator>();

        private void Aplay(string value)
        {
            if(CallbackAnimation!=null)
                CallbackAnimation.Invoke(value);
        }

        public bool IsCurrentAnimation(string name, int layer) => _animator.GetCurrentAnimatorStateInfo(layer).IsName(name); 
        
        public void StartAnimation(string nameAnimation) => _animator.Play(nameAnimation);
    }
}