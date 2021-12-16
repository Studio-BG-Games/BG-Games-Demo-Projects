using System;
using System.Collections.Generic;
using Gameplay.Builds;
using Gameplay.GameSceneScript;
using Gameplay.UI;
using Gameplay.UI.Game;
using Gameplay.UI.Game.Canvas;
using Gameplay.UI.Game.UIElement;
using Gameplay.Units;
using Plugins.DIContainer;
using Plugins.HabObject;
using UnityEngine;
using UnityEngine.UI;

namespace Factorys
{
    public class FactoryUIForGameScene : MonoBehaviour
    {
        [DI] private ContainerUIPRefab _containerUipRefab;
        
        private Dictionary<object, CustomCanvas> _canvas;

        private void Awake() => _canvas=new Dictionary<object, CustomCanvas>();

        public T GetOrCreate<T>(T template, object id) where T : CustomCanvas
        {
            if (_canvas.TryGetValue(id, out var resultDic))
            {
                if (resultDic)
                    return (T) resultDic;
                else
                    _canvas.Remove(id);
            }
            return Create(template, id);
        }

        public T Create<T>(T template, object id) where T : CustomCanvas
        {
            _canvas.Add(id, DiBox.MainBox.CreatePrefab(template));
            return (T) _canvas[id];
        }

        public T CreateUIElement<T>(T template) where T : UIElement => DiBox.MainBox.CreatePrefab(template);
    }
}