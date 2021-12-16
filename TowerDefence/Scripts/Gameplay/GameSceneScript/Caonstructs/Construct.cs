using System;
using System.Collections.Generic;
using System.Linq;
using Factorys;
using Gameplay.Builds;
using Gameplay.HubObject.Data;
using Gameplay.UI.Game.Canvas;
using Interface;
using Plugins.DIContainer;
using UnityEngine;

namespace Gameplay.GameSceneScript
{
    public class Construct : MonoBehaviour
    {
        private Ghost _ghost;

        [SerializeField] private Ghost _ghostTemplate;
        [SerializeField] private List<ConstructState> _states = new List<ConstructState>();
        [Header("маска на блоки")]
        [SerializeField] private LayerMask _laeyrMask;
        
        private Dictionary<Type, ConstructState> _statesDic;
        private ConstructState _currentState;

        [DI] private IInput _input;
        [DI] private ContainerUIPRefab _containerUipRefab;
        [DI] private FactoryUIForGameScene _factoryUiForGameScene;

        private LayoutCanvasWithPoint Canvas => _factoryUiForGameScene.GetOrCreate(_containerUipRefab.CanvasForButton, "CanvasWithButton");

        private void Awake()
        {
            _ghost = DiBox.MainBox.CreatePrefab(_ghostTemplate);
            _ghost.transform.SetParent(transform);
            _ghost.TurnTo(false);
            _states.ForEach(x=>x.Init(_ghost));
            _statesDic = _states.ToDictionary(x => x.GetType());
            Off();
        }

        public void On<T>() where T : ConstructState
        {
            enabled = true;
            _currentState = _statesDic[typeof(T)];
            _currentState.On(Canvas);
        }

        private void Update()
        {
            if (_input.TryRaycstFromPoisitonInput(Mathf.Infinity, _laeyrMask, out RaycastHit hit) && _ghost.enabled)
            {
                if (hit.collider)
                {
                    _ghost.MoveAtPosition(hit.collider.transform.position);
                }
            }
        }

        public void Off()
        {
            enabled = false;
            _currentState?.Off();
            _currentState = null;
            var canvas = Canvas;
            canvas.RemoveAllContent();
            canvas.gameObject.SetActive(false);
        }
    }
}