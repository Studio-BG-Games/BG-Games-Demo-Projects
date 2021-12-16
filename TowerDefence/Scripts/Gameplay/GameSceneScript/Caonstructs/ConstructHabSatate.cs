using System.Collections.Generic;
using Factorys;
using Gameplay.UI;
using Gameplay.UI.Game.Canvas;
using Gameplay.UI.Game.UIElement;
using Interface;
using Plugins.DIContainer;
using Plugins.HabObject;
using UnityEngine;

namespace Gameplay.GameSceneScript
{
    public abstract class ConstructHabSatate : ConstructState
    {
        [DI] protected FactoryUIForGameScene _factoryUi;
        [DI] protected IInput _input;
        [DI] protected ContainerUIPRefab _containerUipRefab;
        [DI] protected Construct _construct;

        private List<ConstructButtonHab> _buttons = new List<ConstructButtonHab>();
        private RollGrid _canvasChoise;

        private bool _isDiing = false;

        protected LayoutCanvasWithPoint CanvasLayer;
        
        public override void On(LayoutCanvasWithPoint canvasForButton)
        {
            CreateButtonChoiseHab();
            CanvasLayer = canvasForButton;
            canvasForButton.RemoveAllContent();

            var but = CreateButtom(_containerUipRefab.AcceptButton, () => OnAccept());
            canvasForButton.AddContent(but.transform, LayoutCanvasWithPoint.Point.Third);
            
            var rotR = CreateButtom(_containerUipRefab.RotateButtonRight, () => Ghost.Rotate(true));
            canvasForButton.AddContent(rotR.transform, LayoutCanvasWithPoint.Point.Fourth);
            
            var rotLeft = CreateButtom(_containerUipRefab.RotateButtonLeft, () => Ghost.Rotate(false));
            canvasForButton.AddContent(rotLeft.transform, LayoutCanvasWithPoint.Point.First);
        }

        protected void CreateButtonChoiseHab()
        {
            List<HabObject> habs = GetHabs();
            _canvasChoise = _factoryUi.GetOrCreate(_containerUipRefab.CanvasConstructButton, "CanvasBuild");
            _canvasChoise.ClearAll();
            for (int i = 0; i < habs.Count; i++)
            {
                var but = _factoryUi.CreateUIElement(GetButtonTemplate(_containerUipRefab));
                but.Init(habs[i]);
                but.Click += OnClick;
                _buttons.Add(but);
                _canvasChoise.AddElement(but, () => but.Activated());
            }

            _canvasChoise.Init();
            _canvasChoise.gameObject.SetActive(true);
        }

        protected abstract ConstructButtonHab GetButtonTemplate(ContainerUIPRefab containerUipRefab);

        protected abstract List<HabObject> GetHabs();

        private void OnAccept()
        {
            Ghost.SetBuild();
            _buttons.ForEach(x=>x.UpdateConditions());
            AddOnAccept();
        }

        protected virtual void AddOnAccept(){}

        private void OnClick(HabObject obj)
        {
            Ghost.TurnTo(true);
            Ghost.Init(obj);
            Ghost.StandBeforeCamera();
            if (!CanvasLayer.gameObject.activeSelf)
            {
                CanvasLayer.gameObject.SetActive(true);    
                CanvasLayer.AttachTo(Ghost.transform);
            }
        }

        public override void Off()
        {
            Ghost.TurnTo(false);
            _canvasChoise.gameObject.SetActive(false);
        }
    }
}