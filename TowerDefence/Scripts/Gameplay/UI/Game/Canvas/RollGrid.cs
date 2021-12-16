using System;
using System.Collections;
using System.Collections.Generic;
using Plugins.DIContainer;
using Plugins.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.UI.Game.Canvas
{
    public class RollGrid : CustomCanvas
    {
        [SerializeField] private Transform _parent;
        [SerializeField] private VerticalLayoutGroup _layoutGroup;
        [SerializeField] private ScrollRect _scrollRect;
        [Min(0)][SerializeField] private float _border;
        private Dictionary<UIElement.UIElement, Action> _callbacks = new Dictionary<UIElement.UIElement, Action>();

        [DI] private ICoroutineRunner _coroutineRunner;

        private float _h;

        public void AddElement<T>(T obj, Action callback) where T : UIElement.UIElement
        {
            _callbacks.Add(obj, callback);
            obj.transform.SetParent(_parent);
        }

        public void ClearAll()
        {
            foreach (var keyValuePair in _callbacks)
            {
                Destroy(keyValuePair.Key.gameObject);
            }
            _callbacks=new Dictionary<UIElement.UIElement, Action>();
        }

        public void Init()
        {
            _coroutineRunner.StartCoroutine(InitLast());
        }

        private IEnumerator InitLast()
        {
            yield return null;
            _h = ((RectTransform) _parent).sizeDelta.y;
            var local = ((RectTransform)_parent.transform).anchoredPosition;
            ((RectTransform)_parent.transform).anchoredPosition = new Vector3(local.x,_h/2);
            _scrollRect.onValueChanged.RemoveListener(OnChange);
            _scrollRect.onValueChanged.AddListener(OnChange);
        }

        private void OnChange(Vector2 pos)
        {
            var rect = ((RectTransform) _parent.transform);
            if (_h > _border + _layoutGroup.spacing*(_callbacks.Count+1))
            {
                if (rect.anchoredPosition.y < _border)
                    rect.anchoredPosition = new Vector2(0, _border);
                else if (rect.anchoredPosition.y > _h - _border)
                    rect.anchoredPosition = new Vector2(0, _h - _border);
            }
            else
            {
                rect.anchoredPosition =  new Vector2(0, Mathf.Clamp(rect.anchoredPosition.y,0,_h));
            }            
        }
    }
}