using System;
using UnityEngine;

namespace Baby_vs_Aliens
{
    public class PlayerAttackState : PlayerState
    {
        #region Fields

        private float _currentFireDelay;
        private Action _callback;
        private bool _isReceivingMoveInput;

        #endregion


        #region ClaaLifeCycles

        public PlayerAttackState(PlayerController controller, Action stateChangeCallback)
        {
            _controller = controller;
            _callback = stateChangeCallback;
        }

        #endregion


        #region IUpdateableRegular

        public override void UpdateRegular()
        {
            if (_currentFireDelay > 0)
                _currentFireDelay -= Time.deltaTime;
            else
                Shoot();

            if (_isReceivingMoveInput)
                _callback?.Invoke();
        }

        #endregion


        #region Methods

        private void Shoot()
        {
            if (!_controller.AreEnemiesPresent)
                return;

            _controller.View.Animator.SetTrigger("Attack");
            var projectile = ServiceLocator.GetService<ObjectPoolManager>().
                GetBulletPool(_controller.Config.ProjectileConfig.Prefab).Pop().GetComponent<Projectile>();

            projectile.Init(_controller.View.BulletSpawn, _controller.View.transform.forward, _controller.Config.ProjectileConfig);
            _controller.View.ShotParticles.Play();
            _currentFireDelay = _controller.Config.ProjectileConfig.FireDelay;
        }

        public override void Reset()
        {
            _controller.View.RigidBody.velocity = Vector3.zero;
            _currentFireDelay = 0.1f;
        }

        public void CheckForMovement(Vector3 inpunValue)
        {
            if (inpunValue != Vector3.zero)
                _isReceivingMoveInput = true;
            else
                _isReceivingMoveInput = false;
        }

        #endregion
    }
}