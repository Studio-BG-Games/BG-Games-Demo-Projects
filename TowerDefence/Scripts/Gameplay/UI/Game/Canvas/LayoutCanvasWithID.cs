using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.UI.Game.Canvas
{
    public class LayoutCanvasWithID : CustomCanvas
    {
        private Dictionary<object, Transform> _objects;
        [SerializeField] private LayoutGroup _content;

        private void Awake() => _objects=new Dictionary<object, Transform>();

        public void AddNewElement(object id, Transform element)
        {
            if(_objects.ContainsKey(id))
                return;
            _objects.Add(id, element);
            element.SetParent(_content.transform);
        }

        public void RemoveElement(object id)
        {
            if (_objects.TryGetValue(id, out var result))
            {
                if(result) 
                    Destroy(result.gameObject);
                _objects.Remove(id);
            }
        }
        
        
    }
}