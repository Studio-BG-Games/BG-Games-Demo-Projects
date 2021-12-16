using System;
using DefaultNamespace;
using Interface;
using Plugins.DIContainer;
using UnityEngine;

namespace Gameplay
{
    [RequireComponent(typeof(Camera))]
    public class ChangerFov : MonoBehaviour
    {
        [Min(0)][SerializeField] private float _speed;
        [Range(0, 180)] [SerializeField] private float _minFov;
        [Range(0, 180)] [SerializeField] private float _maxFov;

        public float Speed
        {
            get => _speed;
            set => _speed = value;
        }

        [DI] private IInput _input;
        
        private Camera _camera;

        private void Awake() => _camera = GetComponent<Camera>();

        private void OnEnable()
        {
            _input.ChangeFov += OnChangeFov;
            ClampFov();
        }

        private void OnDisable()
        {
            _input.ChangeFov -= OnChangeFov;
            ClampFov();
        }

        private void OnChangeFov(float obj)
        {
            _camera.fieldOfView += obj * _speed * Time.deltaTime;
            ClampFov();
        }

        private void ClampFov() => _camera.fieldOfView = Mathf.Clamp(_camera.fieldOfView, _minFov, _maxFov);

        private void OnValidate()
        {
            if (_maxFov < _minFov)
                _maxFov = _minFov;
        }
    }
}