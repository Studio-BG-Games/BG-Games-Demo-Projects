using UnityEngine;

namespace Baby_vs_Aliens
{
    public abstract class EnemyState : IUpdateableRegular
    {
        #region Fields

        protected BaseEnemy _enemy;
        protected bool _isDone;

        #endregion


        #region Properties

        public bool IsDone => _isDone;

        #endregion


        #region IUpdateableRegular

        public abstract void UpdateRegular();

        #endregion


        #region Methods

        protected float GetRandomValue(float min, float max)
        {
            if (max < min)
            {
                max += min;
                min = max - min;
                max -= min;
            }

            return Random.Range(min, max);
        }

        #endregion
    }
}