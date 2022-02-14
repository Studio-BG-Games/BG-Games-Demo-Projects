using UnityEngine;

namespace Baby_vs_Aliens
{
    public class EnemyAttackState : EnemyState
    {
        #region Fields

        private float _attackDuration = 0;
        private bool _isAttacking;
        private bool _hasAttacked;
        private ParticleSystem _attackParticles;
        private Vector3 _targetPosition;

        #endregion


        #region ClassLifeCycles

        public EnemyAttackState(BaseEnemy enemy)
        {
            _enemy = enemy;
            _attackParticles = _enemy.View.GetComponentInChildren<ParticleSystem>();
        }

        #endregion


        #region IUpdateableRegular

        public override void UpdateRegular()
        {
            _targetPosition = _enemy.TargetPosition;
            _targetPosition.y = _enemy.View.transform.position.y;

            RotateTowardsTarget();

            if (_enemy.View.transform.rotation.eulerAngles == Quaternion.LookRotation(_targetPosition - _enemy.View.transform.position).eulerAngles)
            {
                if (!_isAttacking)
                    BeginAttack();
            }

            if (_isAttacking)
                _attackDuration += Time.deltaTime;

            if (_attackDuration > _enemy.Config.AttackDelay && !_hasAttacked)
            {
                Shoot();
                _hasAttacked = true;
            }

            if (_attackDuration > _enemy.Config.AttackRecovery || !_enemy.IsTargetAlive)
            {
                EndState();
            }
        }

        private void RotateTowardsTarget()
        {
            var rotation = Quaternion.RotateTowards(_enemy.View.transform.rotation,
                            Quaternion.LookRotation(_targetPosition - _enemy.View.transform.position), _enemy.Config.AttackRotationSpeed);
            _enemy.View.transform.rotation = rotation;
        }

        #endregion


        #region Methods

        private void BeginAttack()
        {
            _isAttacking = true;
            _enemy.View.Attack();
        }

        private void Shoot()
        {
            var projectile = ServiceLocator.GetService<ObjectPoolManager>().
                GetBulletPool(_enemy.Config.ProjectileConfig.Prefab).Pop().GetComponent<Projectile>();

            var currentPosition = _enemy.View.transform.position;
            currentPosition.y = 0;

            projectile.Init(_enemy.View.BulletSpawn, (_enemy.TargetPosition - currentPosition).normalized, _enemy.Config.ProjectileConfig);

            _attackParticles?.Play();
        }

        private void EndState()
        {
            _enemy.View.ResumePath();
            _enemy.SetMoveState();
            _hasAttacked = false;
            _isAttacking = false;
            _attackDuration = 0;
        }

        #endregion
    }
}