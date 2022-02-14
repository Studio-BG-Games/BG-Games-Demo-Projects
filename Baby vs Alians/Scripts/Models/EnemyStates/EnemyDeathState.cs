using UnityEngine;

namespace Baby_vs_Aliens
{
    public class EnemyDeathState : EnemyState
    {
        #region Fields

        private float _disappearTimer;

        #endregion


        #region ClassLifeCycles

        public EnemyDeathState(BaseEnemy enemy) 
        {
            _enemy = enemy;
            _disappearTimer = _enemy.Config.TimeToStayAfterDeath;
        }

        #endregion


        #region IUpdateableRegular

        public override void UpdateRegular()
        {
            if (_disappearTimer > 0)
                _disappearTimer -= Time.deltaTime;
            else
                _isDone = true;
        }

        #endregion
    }
}