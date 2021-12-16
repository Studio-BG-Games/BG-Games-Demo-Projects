using System;
using System.Collections.Generic;
using Factorys;
using Gameplay.Builds;
using Gameplay.Builds.Data;
using Gameplay.Builds.Data.Marks;
using Gameplay.UI.Game.Canvas;
using Gameplay.UI.Game.UIElement;
using Gameplay.Units;
using Interface;
using Plugins.DIContainer;
using Plugins.HabObject;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Gameplay.GameSceneScript
{
    public class ControlBuildOnMap : MonoBehaviour
    {
        [SerializeField] private LayerMask _layerMask;

        [DI] private FactoryUIForGameScene _factoryUi;
        [DI] private IInput _input;
        [DI] private GridBuild _gridBuild;
        [DI] private FactoryBuild _factoryBuild;
        [DI] private FactoryUnit _factoryUnit;
        [DI] private ContainerUIPRefab _containerUipRefab;
        [DI] private IGold _gold;

        private HabObject _someHab;
        private Vector3 _firstPosition;

        public void On() => enabled = true;
        public void Off() => enabled = false;

        private void OnDisable() => HideMe();

        private void Update()
        {
            if(_input.TryRaycstFromPoisitonInput(1000, _layerMask.value, out var result) && enabled)
            {
                if (result.collider.TryGetComponent<SelectorHabObject>(out var selector))
                {
                    var hab = selector.Get();
                    if(hab.MainDates.GetOrNull<NeksusMark>())
                        return;
                    ViewHub(hab);
                    _firstPosition =hab.transform.position;
                    CreateCanvasWithButton();
                }
                else if(_someHab!=null)
                {
                    var posToMove = result.transform.position;
                    MoveHubToPosition(_someHab, posToMove);
                    _gridBuild.VieaArenaAsync(_someHab);
                }
            }
        }

        private void MoveHubToPosition(HabObject habObject, Vector3 posToMove)
        {
            if (habObject is Build)
            {
                if (habObject.MainDates.GetOrNull<NeksusMark>() == null)
                {
                    _factoryBuild.TryMove((Build) habObject, posToMove);
                }
            }
            else if (habObject is Unit)
            {
                _factoryUnit.TryMove((Unit) habObject, posToMove);
            }
            
        }

        private void CreateCanvasWithButton()
        {
            var canvas = _factoryUi.GetOrCreate(_containerUipRefab.CanvasForButton, "canvas for button control");
            canvas.RemoveAllContent();
            canvas.gameObject.SetActive(true);
            canvas.AttachTo(_someHab.transform);
            
            var accept = CreateButtom(_containerUipRefab.AcceptButton, HideMe);
            var canvel = CreateButtom(_containerUipRefab.CancelButton, () =>
            {
                MoveHubToPosition(_someHab, _firstPosition);
                HideMe();
            });
            var deleteButton = CreateButtom(_containerUipRefab.DeleteButton, () => DeleteObject(_someHab));
            
            canvas.AddContent(accept.transform, LayoutCanvasWithPoint.Point.Fourth);
            canvas.AddContent(canvel.transform, LayoutCanvasWithPoint.Point.First);
            canvas.AddContent(deleteButton.transform, LayoutCanvasWithPoint.Point.Second);
        }

        private void DeleteObject(HabObject hab)
        {
            var cost = hab.MainDates.GetOrNull<Cost>().Gold;
            if(hab.MainDates.GetOrNull<NeksusMark>()!=null)
                return;
            _gold.Add(cost);
            HideMe();
            Destroy(hab.gameObject);
        }

        private UIButton CreateButtom(UIButton button, Action callback)
        {
            var but = _factoryUi.CreateUIElement(button);
            but.Button.onClick.AddListener(()=>callback?.Invoke());
            return but;
        }

        private void HideMe()
        {
            _gridBuild.HideAll();
            _someHab = null;
            _factoryUi.GetOrCreate(_containerUipRefab.CanvasForButton, "canvas for button control").gameObject.SetActive(false);
        }

        private void ViewHub(HabObject someBuild)
        {
            _gridBuild.VieaArenaAsync(someBuild);
            _someHab = someBuild;
        }
    }
}