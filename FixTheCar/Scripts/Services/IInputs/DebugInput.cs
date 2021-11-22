using System;
using Services.Interfaces;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Services.IInputs
{
    public class DebugInput : IInput, IShowHide
    {
        private CanvasUiInput _canvasInput;
        public event Action AnyInput;
        public event Action<Vector3> RayCastClickOnScreen;
        public event Action<Vector2> RayCastInGameField;
        public event Action<float> NormalizeHorizontalMove;
        public Vector3 InputScreenPosition => Input.mousePosition;

        private int _directionMoveX = 0;
         
        public DebugInput(CanvasUiInput canvasInput)
        {
            _canvasInput = canvasInput;
            _canvasInput.LeftButton.ClickDown += LeftDown;
            _canvasInput.LeftButton.ClickUp += LeftUp;
            _canvasInput.RightButton.ClickDown += RightDown;
            _canvasInput.RightButton.ClickUp += RightUp;
            _canvasInput.Hide();
        }

        private void RightUp()
        {
            if (_directionMoveX == 1) _directionMoveX = 0;
        }

        private void RightDown() => _directionMoveX = 1;

        private void LeftUp()
        {
            if (_directionMoveX == -1) _directionMoveX = 0;
        }

        private void LeftDown() => _directionMoveX = -1;

        public void Update()
        {
            if(Input.anyKey) AnyInput?.Invoke();
            if (Input.GetMouseButtonDown(0))
            {
                RayCastInGameField?.Invoke(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                if(EventSystem.current.IsPointerOverGameObject())
                    return;
                RayCastClickOnScreen?.Invoke(Input.mousePosition);
            }
            NormalizeHorizontalMove?.Invoke(_directionMoveX);
        }

        public void Show() => _canvasInput.Show();

        public void Hide() => _canvasInput.Hide();
    }
}