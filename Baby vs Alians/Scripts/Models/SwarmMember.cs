using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Baby_vs_Aliens
{
    public class SwarmMember : BaseEnemy, ISwarmMember
    {
        #region Fields

        private ISwarm _swarm;

        #endregion


        #region ClassLifeCycles

        public SwarmMember(ISwarm swarm, bool isSwarmLeader, ArenaController arenaController, PlayerController playerController, Action<int> scoreCallback, EnemyType type) :
            base(arenaController, playerController, scoreCallback, type)
        {
            _swarm = swarm;
            IsSwarmLeader = isSwarmLeader;

            if (!IsSwarmLeader)
                _view.transform.position = GetRandomPositionAroundLeader();
        }

        public bool IsSwarmLeader { get; set; }

        public Vector3 LeaderPosition => _swarm.LeaderPosition;

        public Vector3 Position => _view.transform.position;

        #endregion


        #region ISwarmMember

        public Vector3 GetRandomPositionAroundLeader()
        {
            var leaderPosition = LeaderPosition;
            var offsetDirection = Random.insideUnitCircle.normalized;
            var position = new Vector3(leaderPosition.x + offsetDirection.x,
                0,
                leaderPosition.z + offsetDirection.y);

            return position;
        }

        #endregion


        #region Methods

        protected override void InitStates()
        {
            _moveState = new EnemySwarmMoveState(this);
            _attackState = new EnemyAttackState(this);
            _deathState = new EnemyDeathState(this);

            SetMoveState();
        }

        #endregion
    }
}