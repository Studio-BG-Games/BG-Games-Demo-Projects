using System;
using UnityEngine;

namespace Baby_vs_Aliens
{
    public class PlayerMoveState : PlayerState
    {
        #region Fields

        private Vector3 _moveVector;
        private Action _callback;

        #endregion


        #region ClaaLifeCycles

        public PlayerMoveState(PlayerController controller, Action stateChangeCallback)
        {
            _controller = controller;
            _callback = stateChangeCallback;
        }

        #endregion


        #region IUpdateableRegular

        public override void UpdateRegular()
        {
            MovePlayer();
        }

        #endregion


        #region Methods

        private void MovePlayer()
        {
            _controller.View.SetMoveVector(_moveVector.normalized * _controller.Config.Speed * Time.deltaTime);

            SetAnimatorState();
        }

        private void SetAnimatorState()
        {
            if (_moveVector.x != 0 || _moveVector.z != 0)
                _controller.View.SetState(CharacterState.Run);
            else
            {
                _controller.View.SetState(CharacterState.Idle);
                _callback?.Invoke();
            }
        }

        public void SetMoveVector(Vector3 moveVector)
        {
            _moveVector = moveVector;
        }

        public override void Reset()
        {
            SetAnimatorState();
        }

        #endregion

    }
}