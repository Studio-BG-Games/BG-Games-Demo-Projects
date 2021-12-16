using System;
using System.Collections;
using Interface;
using Plugins.DIContainer;
using Plugins.Interfaces;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Services.Inputs
{
    public class PCInput : IInput
    {
        [DI] private ICoroutineRunner _coroutineRunner;
        
        private Camera _main;
        private Camera MainCamera => _main!=null?_main:_main=Camera.main;

        public event Action<Vector3> MoveCameraAtDiraction;
        public event Action<float> ChangeFov;

        [DI]
        private void Init()
        {
            _coroutineRunner.StartCoroutine(Update());
        }

        private IEnumerator Update()
        {
            while (true)
            {
                if(CheckMoveCamera(out var dir))
                    MoveCameraAtDiraction?.Invoke(dir);
                if(CheckChangeFov(out var value))
                    ChangeFov?.Invoke(value);
                yield return null;
            }
        }

        private bool CheckChangeFov(out float o)
        {
            o = 0;
            if (Input.GetKey(KeyCode.RightArrow))
                o = 1;
            if (Input.GetKey(KeyCode.LeftArrow))
                o = -1;
            return o != 0;
        }


        private StateMouse _state;
        private Vector3 _lastPos;
        private float _time;
        private bool CheckMoveCamera(out Vector3 dir)
        {
            dir = Vector3.zero;
            if(Input.GetMouseButtonDown(0))
                _state = StateMouse.SetPos;
            if (_time < 0.83f)
            {
                _time += Time.deltaTime;
                return false;
            }

            if (_state == StateMouse.SetPos)
            {
                _lastPos = Input.mousePosition;
                _state = StateMouse.CalDir;
            }
            else if (_state == StateMouse.CalDir)
            {
                dir = _lastPos - Input.mousePosition;
                dir.z = dir.y;
                dir.y = 0;
                dir = dir.normalized;
                _lastPos = Input.mousePosition;
            }

            if (Input.GetMouseButtonUp(0))
                _time = 0;
            return Input.GetMouseButton(0);
        }

        public bool TryRaycstFromPoisitonInput(float lenght, LayerMask laeyrMask, out RaycastHit raycastHit)
        {
            raycastHit = new RaycastHit();
            if (!UnityEngine.Input.GetMouseButtonDown(0) || EventSystem.current.IsPointerOverGameObject())
                return false;
            Ray ray = MainCamera.ScreenPointToRay(UnityEngine.Input.mousePosition);
            return Physics.Raycast(ray, out raycastHit, lenght, laeyrMask.value);
        }
    }

    internal enum StateMouse
    {
        SetPos, CalDir
    }
}