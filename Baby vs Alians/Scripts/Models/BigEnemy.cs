using System;

namespace Baby_vs_Aliens
{
    public class BigEnemy : BaseEnemy
    {


        #region ClassLifeCycles

        public BigEnemy(ArenaController arenaController, PlayerController playerController, Action<int> scoreCallback, EnemyType type) :
            base(arenaController, playerController, scoreCallback, type){}

        #endregion


        #region Methods

        protected override void InitStates()
        {
            _moveState = new EnemyMoveState(this);
            _attackState = new EnemyAttackState(this);
            _deathState = new EnemyDeathState(this);

            SetMoveState();
        }

        #endregion
    }
}