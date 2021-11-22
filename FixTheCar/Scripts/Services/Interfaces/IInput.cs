using System;
using UnityEngine;

namespace Services.Interfaces
{
    public interface IInput
    {
        event Action AnyInput;
        event Action<Vector3> RayCastClickOnScreen;
        event Action<Vector2> RayCastInGameField;
        event Action<float> NormalizeHorizontalMove;
        public Vector3 InputScreenPosition { get; }

        void Update();
    }
}