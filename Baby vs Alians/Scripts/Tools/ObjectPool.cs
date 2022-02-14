using System.Collections.Generic;
using UnityEngine;

namespace Baby_vs_Aliens
{
    public sealed class ObjectPool
    {
        #region Fields

        private readonly Stack<GameObject> _stack = new Stack<GameObject>();
        private readonly GameObject _prefab;
        private Transform _parentObject;

        #endregion


        #region ClassLifeCycles

        public ObjectPool(GameObject prefab, Transform parentObject)
        {
            _prefab = prefab;
            _parentObject = parentObject;
        }

        #endregion


        #region Methods

        public void Push(GameObject gameObject)
        {
            _stack.Push(gameObject);
            gameObject.SetActive(false);
        }

        public GameObject Pop()
        {
            GameObject gameObject;
            if (_stack.Count == 0)
            {
                gameObject = Object.Instantiate(_prefab);
                gameObject.transform.parent = _parentObject;
            }
            else
            {
                gameObject = _stack.Pop();
            }
            gameObject.SetActive(true);

            return gameObject;
        }

        #endregion
    }
}