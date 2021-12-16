using System;
using System.Collections;
using Interface;
using Plugins.DIContainer;
using Plugins.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Services.Inputs
{
    public class MobileInput : IInput
    {
        [DI] private ICoroutineRunner _coroutineRunner;
        
        private Camera _main;
        private Camera MainCamera => _main!=null?_main:_main=Camera.main;

        public event Action<Vector3> MoveCameraAtDiraction;
        public event Action<float> ChangeFov;

        [DI]
        private void Init()
        {
            _coroutineRunner.StartCoroutine(LateUpdate());
        }

        private IEnumerator LateUpdate()
        {
            while (true)
            {
                if(CheckMoveCamera(out var dir))
                    MoveCameraAtDiraction?.Invoke(dir); 
                if(CheckChangeFov(out var value))
                    ChangeFov?.Invoke(value);
                yield return new WaitForFixedUpdate();
            }
        }

        public bool TryRaycstFromPoisitonInput(float lenght, LayerMask laeyrMask, out RaycastHit raycastHit)
        {
            raycastHit = new RaycastHit();
            if (UnityEngine.Input.touchCount > 0)
            {
                if (EventSystem.current.IsPointerOverGameObject(UnityEngine.Input.GetTouch(0).fingerId))
                    return false;
                if (Input.GetTouch(0).phase != TouchPhase.Began)
                    return false;
                Ray ray = MainCamera.ScreenPointToRay(UnityEngine.Input.mousePosition);
                return Physics.Raycast(ray, out raycastHit, lenght, laeyrMask.value);
            }
            return false;
        }

        private bool CheckChangeFov(out float o)
        {
            o = 0;
            if (Input.touchCount != 2)
                return false;
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            // Find the position in the previous frame of each touch.
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevTouchLengh = Vector2.Distance(touchZeroPrevPos, touchOnePrevPos);
            float currentTouchLengh = Vector2.Distance(touchOne.position, touchZero.position);

            o = prevTouchLengh - currentTouchLengh;
            o *= 0.03f;
            
            // Find the magnitude of the vector (the distance) between the touches in each frame.
            //float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            //float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            // Find the difference in the distances between each frame.
            //o = prevTouchDeltaMag - touchDeltaMag;
            
            return true;
        }

        private bool CheckMoveCamera(out Vector3 dir)
        {
            dir = Vector3.zero;
            if (Input.touchCount == 0 || Input.touchCount>1)
                return false;
            if (EventSystem.current.IsPointerOverGameObject(UnityEngine.Input.GetTouch(0).fingerId))
                return false;
            var delta = -Input.GetTouch(0).deltaPosition;
            dir = new Vector3(delta.x, 0 , delta.y);
            dir = dir.normalized;
            return true;
        }
    }
}