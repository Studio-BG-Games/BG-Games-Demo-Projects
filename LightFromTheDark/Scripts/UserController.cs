using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserController : MonoBehaviour
{
    [SerializeField] private PlayerController _playerControl;
    [SerializeField] bool _keyboardControl;
    [SerializeField] bool _buttonsControl;
    [SerializeField] bool _acceletarionControl;

    private bool _pressedRunRight;
    private bool _pressedRunLeft;
    private bool _pressedJump;

    private Vector3 _startAcceleration;


    private void FixedUpdate()
    {
        if(_startAcceleration == Vector3.zero)
        {
            _startAcceleration = Input.acceleration;
        }

        if (_keyboardControl)
        {
            if (Input.GetAxisRaw("Horizontal") != 0)
                _playerControl.Run(Input.GetAxis("Horizontal") > 0 ? PlayerController.DirectionMove.right : PlayerController.DirectionMove.left);
            if (Input.GetAxisRaw("Jump") != 0)
            {
                if (_playerControl.InClimpbingZone)
                    _playerControl.IsClipmbing = true;
                else
                    _playerControl.Jump();
            }
            else
            {
                if (_playerControl.InClimpbingZone)
                    _playerControl.IsClipmbing = false;
            }
        }
        if(_buttonsControl)
        {
            if (_pressedRunRight)
                _playerControl.Run(PlayerController.DirectionMove.right);
            else if (_pressedRunLeft)
                _playerControl.Run(PlayerController.DirectionMove.left);

            if (_pressedJump)
            {
                if (_playerControl.InClimpbingZone)
                    _playerControl.IsClipmbing = true;
                else
                {
                    _playerControl.Jump();
                    _pressedJump = false;
                }
            }
            else
            {
                if (_playerControl.InClimpbingZone)
                    _playerControl.IsClipmbing = false;
            }
        }
        if(_acceletarionControl)
        {
            float her;
            Vector3 acceleration = Input.acceleration;
            Vector3 dir = acceleration - _startAcceleration;
            if (Mathf.Abs(dir.y) > Mathf.Abs(dir.z))
                her = Mathf.Abs(dir.y);
            else
                her = Mathf.Abs(dir.z);
            if (dir.y < 0)
                her *= -1;

            acceleration.z = 0;
            acceleration.y = her;

            if (acceleration.magnitude > 0.2f)
                _playerControl.AccelerationMove(acceleration);
        }

    }

    public void OnDownButtonRunRight()
    {
        _pressedRunRight = true;
    }
    public void OnUpButtonRunRight()
    {
        _pressedRunRight = false;
    }

    public void OnDownButtonRunLeft()
    {
        _pressedRunLeft = true;
    }
    public void OnUpButtonRunLeft()
    {
        _pressedRunLeft = false;
    }

    public void OnDownButtonJump()
    {
        _pressedJump = true;
    }
    public void OnUpButtonJump()
    {
        _pressedJump = false;
    }
}
