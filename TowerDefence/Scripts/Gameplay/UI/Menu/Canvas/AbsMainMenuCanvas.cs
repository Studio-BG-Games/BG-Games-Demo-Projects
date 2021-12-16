using System;
using UnityEngine;

namespace Gameplay.UI.Menu.Canvas
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class AbsMainMenuCanvas : MonoBehaviour
    {
        public Transform RootPanel => _rootPanel;
        [SerializeField] private Transform _rootPanel;

        public CanvasGroup CanvasGroup => _canvasGroup;
        private CanvasGroup _canvasGroup;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            CustomAwake();
        }
        
        protected virtual void CustomAwake(){}
    }
}