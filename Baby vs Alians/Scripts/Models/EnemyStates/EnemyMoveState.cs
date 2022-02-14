using UnityEngine;

namespace Baby_vs_Aliens
{
    public class EnemyMoveState : EnemyState
    {
        #region Fields

        protected float _rerouteTimer;
        protected float _attackTimer;
        protected bool _hasBeenInterrupted;

        #endregion


        #region ClassLifeCycles

        public EnemyMoveState(BaseEnemy enemy)
        {
            _enemy = enemy;
            ResetAttackTimer();
        }

        #endregion


        #region IUpdateableRegular

        public override void UpdateRegular()
        {
            if (_hasBeenInterrupted)
            {
                ResumeMovement();
            }

            if (_rerouteTimer > 0)
                _rerouteTimer -= Time.deltaTime;

            if (_rerouteTimer <= 0 || _enemy.Config.RerouteOnDestinationReached && _enemy.View.HasReachedDestination)
                NewDestination();

            if (_attackTimer > 0)
                _attackTimer -= Time.deltaTime;
            else
            {
                ResetAttackTimer();
                EndState();
            }

            if (!_enemy.IsTargetAlive)
                ResetAttackTimer();
        }

        #endregion


        #region Methods

        protected void ResetAttackTimer()
        {
            _attackTimer = GetRandomValue(_enemy.Config.MinTimeBetweenAttacks, _enemy.Config.MaxTimeBetweenAttacks);
        }

        protected virtual void ResetRerouteTimer()
        {
            _rerouteTimer = GetRandomValue(_enemy.Config.MinTimeBeforeReroute, _enemy.Config.MaxTimeBeforeReroute);
        }

        protected virtual void NewDestination()
        {
            _enemy.View.SetDestination(_enemy.ArenaController.GetRandomPosition());
            ResetRerouteTimer();
        }

        protected virtual void ResumeMovement()
        {
            _enemy.View.ResumePath();
            _hasBeenInterrupted = false;
        }

        private void EndState()
        {
            _enemy.View.Stop();
            _enemy.SetAttackState();
            _hasBeenInterrupted = true;
        }

        #endregion
    }
}