using System;
using UnityEngine;

namespace Gameplay
{
    public class RotateToMainCamera : MonoBehaviour
    {
        [SerializeField] private bool _onlyY = false;
        private Camera _camera;

        private void Awake() => _camera = Camera.main;

        private void LateUpdate()
        {
            if(_onlyY)
                RotateByY();
            else 
                transform.LookAt(_camera.transform);
        }

        private void RotateByY()
        {
            var dir = (_camera.transform.position - transform.position).normalized;
            dir.y = 0;
            Quaternion lookRot = Quaternion.LookRotation(dir);
            transform.rotation = lookRot;
        }
    }
}