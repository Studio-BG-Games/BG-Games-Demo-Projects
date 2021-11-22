using System;
using System.Collections;
using Plugins.DIContainer;
using Plugins.Interfaces;
using UnityEngine;

namespace Mechanics.Prompters.Interfaces
{
    [CreateAssetMenu(menuName = "Prometed/delay type/timer", order = 51)]
    public class TimingDelayPrometed : AbsDelayPrometed
    {
        [Min(0)][SerializeField] private float _delayInSecond = 2; 
        
        public override void Activated(Action callback)
        {
            DiBox.MainBox.ResolveSingle<ICoroutineRunner>().StartCoroutine(StartTime(callback));
        }

        private IEnumerator StartTime(Action callback)
        {
            yield return new WaitForSeconds(_delayInSecond);
            callback?.Invoke();
        }
    }
}