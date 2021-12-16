using System;
using System.Collections.Generic;
using Gameplay.UI.Game.UIElement;
using Plugins.HabObject;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.UI
{
    [RequireComponent(typeof(Button))]
    public class ConstructButtonHab : UIElement
    {
        private HabObject _habTarget;
        private Button _button;

        public event Action<HabObject> Click;

        [SerializeField] private List<PartInitConstructButton> _constructButtonHab = new List<PartInitConstructButton>();
        
        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(Clicked);
        }

        public void Activated() => Clicked();
        
        public void UpdateConditions() => _constructButtonHab.ForEach(x=>x.Init(_habTarget));
        
        private void Clicked()
        {
            Click?.Invoke(_habTarget);
        }

        public void Init<T>(T template) where T : HabObject
        {
            _habTarget = template;
            UpdateConditions();
        }
    }
}