using UnityEngine;

namespace Baby_vs_Aliens
{
    public class EnemySwarmMoveState : EnemyMoveState
    {
        #region Properties

        private bool IsSwarmMember => _enemy is ISwarmMember;

        #endregion

        #region ClassLifeCycles

        public EnemySwarmMoveState(SwarmMember enemy) : base(enemy)
        {
            _enemy = enemy;
            ResetAttackTimer();
        }

        #endregion


        #region Methods

        protected override void NewDestination()
        {
            if (!IsSwarmMember || IsSwarmMember && (_enemy as ISwarmMember).IsSwarmLeader)
                base.NewDestination();
            else
            {
                var destination = (_enemy as ISwarmMember).GetRandomPositionAroundLeader();
                _enemy.View.SetDestination(destination);
                _rerouteTimer = GetRandomValue(_enemy.Config.MinTimeBeforeReroute, _enemy.Config.MaxTimeBeforeReroute);
            }
        }

        protected override void ResumeMovement()
        {
            if (!IsSwarmMember || IsSwarmMember && (_enemy as ISwarmMember).IsSwarmLeader)
                base.ResumeMovement();
            else
            {
                _hasBeenInterrupted = false;
                NewDestination();
            }
        }

        protected override void ResetRerouteTimer()
        {
            base.ResetRerouteTimer();
            if (IsSwarmMember && !(_enemy as ISwarmMember).IsSwarmLeader)
                _rerouteTimer /= 4;
        }

        #endregion
    }
}