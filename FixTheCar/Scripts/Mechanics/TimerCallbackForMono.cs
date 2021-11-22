using System;
using System.Collections;
using System.Threading.Tasks;
using Plugins.DIContainer;
using Plugins.Interfaces;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Mechanics
{
    [System.Serializable]
    public class TimerCallbackForMono
    {
        [Min(0.0001f)] private float _time;

        public void Start(Action callback) => DiBox.MainBox.ResolveSingle<ICoroutineRunner>().StartCoroutine(Timing(callback));

        private IEnumerator Timing(Action callback)
        {
            yield return new WaitForSeconds(_time);
            callback?.Invoke();
        }
    }
}