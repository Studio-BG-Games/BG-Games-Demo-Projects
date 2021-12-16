using System;
using Gameplay.Builds;
using Gameplay.UI.Game.Canvas;
using Gameplay.UI.Game.UIElement;
using Plugins.DIContainer;
using UnityEngine;

namespace Gameplay.GameSceneScript
{
    public abstract class ConstructState : MonoBehaviour
    {
        protected Ghost Ghost;

        public void Init(Ghost ghost) => Ghost = ghost;
        
        public abstract void On(LayoutCanvasWithPoint canvasForButton);
        public abstract void Off();

        protected UIButton CreateButtom(UIButton template, Action callback)
        {
            var but = DiBox.MainBox.CreatePrefab(template);
            but.Button.onClick.AddListener(()=>callback?.Invoke());
            return but;
        }
    }
}