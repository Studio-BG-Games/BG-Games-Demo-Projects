using System;
using UnityEngine;

namespace Baby_vs_Aliens
{
    public class PlayerDamagedState : PlayerState
    {
        #region Fields

        private Action _callback;
        private float _currentRecoveryTimer;

        #endregion

        #region ClaaLifeCycles

        public PlayerDamagedState(PlayerController controller, Action stateChangeCallback)
        {
            _controller = controller;
            _callback = stateChangeCallback;
        }

        #endregion


        #region IUpdateaableRegular

        public override void UpdateRegular()
        {
            if (_currentRecoveryTimer > 0)
            {
                _currentRecoveryTimer -= Time.deltaTime;
            }
            else
                _callback?.Invoke();
        }

        #endregion


        #region Methods

        public override void Reset()
        {
            _currentRecoveryTimer = _controller.Config.DamageRecoveryTime;
            _controller.View.RigidBody.velocity = Vector3.zero;
            _controller.View.SetState(CharacterState.Damaged);
        }

        #endregion
    }
}