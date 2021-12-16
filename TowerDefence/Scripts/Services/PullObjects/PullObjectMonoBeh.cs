using System;
using System.Collections.Generic;
using Plugins.DIContainer;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Services.PullObjects
{
    public class PullObjectMonoBeh<T> where T : Component
    {
        private List<T> _busy = new List<T>();
        private List<T> _free = new List<T>();

        private T _template;
        private Transform _parent;

        public PullObjectMonoBeh(T template, Transform parent, int baseCount=2)
        {
            if (baseCount < 2)
                baseCount = 2;
            _template = template;
            _parent = parent;
            if(!template) throw new Exception("template is null");
            ExpandFree(baseCount);
        }

        public T GetElement()
        {
            if (_free.Count == 0)
                ExpandFree(_busy.Count / 2);
            var first = _free[0];
            _free.Remove(first);
            _busy.Add(first);
            first.gameObject.SetActive(true);
            return first;
        }

        public void ReturnElement(T elelment)
        {
            elelment.gameObject.SetActive(false);
            _busy.Remove(elelment);
            _free.Add(elelment);
        }

        private void ExpandFree(int busyCount)
        {
            for (int i = 0; i < busyCount; i++)
            {
                _free.Add(Object.Instantiate(_template, Vector3.zero, Quaternion.identity, _parent));
                _free[i].transform.eulerAngles = _template.transform.eulerAngles;
                _free[i].gameObject.SetActive(false);
            }
        }
    }
}