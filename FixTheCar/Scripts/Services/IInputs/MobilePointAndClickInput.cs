using System;
using Services.Interfaces;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Infrastructure.LevelState.States
{
    public class MobilePointAndClickInput : IInput
    {
        private Vector2 _lastPosition;
        public event Action AnyInput;
        public event Action<Vector3> RayCastClickOnScreen;
        public event Action<Vector2> RayCastInGameField;
        public event Action<float> NormalizeHorizontalMove;
        public Vector3 InputScreenPosition => GetScreenPositionByLastClick();
        
        private Vector3 GetScreenPositionByLastClick()
        {
            if (Input.touchCount > 0) _lastPosition = Input.GetTouch(0).position;
            return _lastPosition;
        }
        
        public void Update()
        {
            if (Input.touchCount>0) AnyInput?.Invoke();
            if (Input.touchCount>0)
            {
                RayCastInGameField?.Invoke(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                    return;
                RayCastClickOnScreen?.Invoke(Input.GetTouch(0).position);
            }
        }
    }
}