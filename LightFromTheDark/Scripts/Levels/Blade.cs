using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blade : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private GameObject _platformObj;

    private bool _isCutting;
    private bool _leftCut;
    private bool _rightCut;
    private bool _isFall;

    private void Update()
    {
        if(Input.GetMouseButtonDown(0) == true)
        {
            StartCutting();
        }
        else if(Input.GetMouseButtonUp(0) == true)
        {
            StopCutting();
        }

        if(_isCutting == true)
        {
            UpdateCutting();
        }
        if(_isFall == false && _rightCut == true && _leftCut == true)
        {
            _isFall = true;
            _platformObj.GetComponent<Animator>().Play("Fall");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.name == "Chain Right")
        {
            _rightCut = true;
        }
        else if (collision.name == "Chain Left")
        {
            _leftCut = true;
        }
    }

    private void StartCutting()
    {
        _isCutting = true;
    }

    private void StopCutting()
    {
        _isCutting = false;
        _leftCut = false;
        _rightCut = false;
    }

    private void UpdateCutting()
    {
        transform.position = _mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _mainCamera.nearClipPlane));
    }
}
