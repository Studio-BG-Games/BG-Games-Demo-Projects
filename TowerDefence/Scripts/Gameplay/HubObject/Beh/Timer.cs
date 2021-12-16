using System;
using System.Collections;
using UnityEngine;

namespace DefaultNamespace
{
    public class Timer : MonoBehaviour
    {
        public event Action Started;
        public event Action Finished;
        public event Action Stoped;

        private Coroutine _action;

        public void StartMe(float time)
        {
            if (time <= 0)
            {
                Started?.Invoke();
                Finished?.Invoke();
            }
            else
            {
                _action = StartCoroutine(StartTime(time));
            }
        }

        public void StopMe()
        {
            if (_action != null)
            {
                StopCoroutine(_action);
                _action = null;
                Stoped?.Invoke();
            }
        }

        private IEnumerator StartTime(float time)
        {
            Started?.Invoke();
            yield return new WaitForSeconds(time);
            _action = null;
            Finished?.Invoke();
        }
    }
}