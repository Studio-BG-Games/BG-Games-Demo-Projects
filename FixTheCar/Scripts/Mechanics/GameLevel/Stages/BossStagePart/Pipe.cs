using System;
using System.Collections;
using UnityEngine;

namespace Mechanics.GameLevel.Stages.BossStagePart
{
    public class Pipe : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _particleSystem;
        [Min(0)] [SerializeField] private float _callbackDelay;
        
        public void MakeFlow(Action action)
        {
            _particleSystem.Play();
            StartCoroutine(InvokeCallbackWithDelay(action));
        }

        private IEnumerator InvokeCallbackWithDelay(Action callback)
        {
            yield return new WaitForSeconds(_callbackDelay);
            callback?.Invoke();
        }
    }
}