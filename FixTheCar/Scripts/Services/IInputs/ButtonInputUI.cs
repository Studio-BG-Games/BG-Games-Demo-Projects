using System;
using DefaultNamespace.Services.Console;
using ExtendedButtons;
using Plugins.DIContainer;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Services.IInputs
{
    [RequireComponent(typeof(Button2D))]
    public class ButtonInputUI : MonoBehaviour
    {
        private Button2D _button2D;
        
        [DI] private IConsole _console;
        
        public event Action ClickUp;
        public event Action ClickDown;
        
        private void Awake()
        {
            _button2D = GetComponent<Button2D>();
            _button2D.onDown.AddListener(() =>
            {
                _console.Log($"Click down - {gameObject.name}");
                ClickDown?.Invoke();
            });
            _button2D.onUp.AddListener(()=>
            {
                _console.Log($"Click up - {gameObject.name}");
                ClickUp?.Invoke();
            });
        }
    }
}