using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSpriteMask : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;

    void Update()
    {
        if (Input.touchCount > 0 && Time.timeScale != 0)
        {
            Touch touch = Input.GetTouch(0);
            transform.position = _mainCamera.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, _mainCamera.nearClipPlane));
        }

    }
}
