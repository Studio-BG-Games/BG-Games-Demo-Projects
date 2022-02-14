using System;
using UnityEngine;

namespace Baby_vs_Aliens
{
    [Serializable]
    public struct EnemyAmount
    {
        #region Fields

        [Min(1)] public int BigEnemyAmount;
        [Min(1)] public int SmallEnemyAmount;
        [Min(1)] public int LevelsToAddBigEnemy;
        [Min(1)] public int LevelsToAddSmallEnemy;

        #endregion
    }
}