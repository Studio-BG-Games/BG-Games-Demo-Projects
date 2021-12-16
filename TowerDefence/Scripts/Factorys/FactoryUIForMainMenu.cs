using System;
using System.Collections.Generic;
using DefaultNamespace.Infrastructure.Data;
using Gameplay.UI.Menu.Canvas;
using Plugins.DIContainer;
using Plugins.HabObject;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Gameplay.UI.Menu
{
    public class FactoryUIForMainMenu : MonoBehaviour
    {
        private Dictionary<Type, object> _dictionary =new Dictionary<Type, object>();
        
        public T GetOrCreate<T>(T template) where T : AbsMainMenuCanvas
        {
            if (_dictionary.ContainsKey(typeof(T)))
                return _dictionary[typeof(T)] as T;
            var canvas = DiBox.MainBox.CreatePrefab(template);
            _dictionary.Add(typeof(T), canvas);
            return canvas;
        }

        public ViewProfile<Card, T, TData> CreateViewProfile<Card, T, TData>(ViewProfile<Card, T, TData> profile) 
            where Card : ObjectCardProfile<T, TData> 
            where T : Object 
            where TData : SaveDataProfile
        {
            var t = DiBox.MainBox.CreatePrefab(profile);
            DiBox.MainBox.InjectSingle(t.gameObject);
            return t;
        }
    }
}