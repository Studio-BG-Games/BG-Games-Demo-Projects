using System;
using UnityEngine;

namespace Baby_vs_Aliens
{
    public class PlayerDeathState : PlayerState
    {
        #region Fields

        private float _currentRespawnTimer;
        private Action _callback;
        private bool _isHidden;

        #endregion


        #region ClaaLifeCycles

        public PlayerDeathState(PlayerController controller, Action stateChangeCallback)
        {
            _controller = controller;
            _callback = stateChangeCallback;
        }

        #endregion


        #region IUpdateableRegular

        public override void UpdateRegular()
        {
            if (_currentRespawnTimer > 0)
            {
                _currentRespawnTimer -= Time.deltaTime;

                if (_currentRespawnTimer < _controller.Config.RespawnTime / 2 && !_isHidden)
                    HideModel();
            }
            else
                _callback?.Invoke();
        }

        #endregion


        #region Methods

        private void HideModel()
        {
            _isHidden = true;

            _controller.View.HideCharacter();
            _controller.View.DeathParticles.Play();
        }

        public override void Reset()
        {
            _currentRespawnTimer = _controller.Config.RespawnTime;
            _isHidden = false;
            _controller.View.SetState(CharacterState.Death);
        }

        #endregion
    }
}