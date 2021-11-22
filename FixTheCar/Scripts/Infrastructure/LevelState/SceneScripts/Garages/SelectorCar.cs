using System;
using JetBrains.Annotations;
using Mechanics.Garage;
using Plugins.DIContainer;
using Services.Interfaces;
using UnityEngine;

namespace Infrastructure.LevelState.SceneScripts.Garages
{
    public class SelectorCar
    {
        [CanBeNull] public Car SelectedCar { get; private set; }
        [CanBeNull] public event Action<Car> NewCarSelect;
        
        [DI] private IInput _input;

        public void On() => _input.RayCastClickOnScreen += OnRayCastClick;

        public void Off() => _input.RayCastClickOnScreen -= OnRayCastClick;

        private void OnRayCastClick(Vector3 pointScreen)
        {
            var collisions = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(pointScreen), Vector2.zero);
            foreach (var collision in collisions)
            {
                if (collision.collider.TryGetComponent<Car>(out var result))
                {
                    InvokeEvent(result);
                    return;
                }
            }
            InvokeEvent(null);
        }

        private void InvokeEvent(Car result) => NewCarSelect?.Invoke(SelectedCar = result);
    }
}