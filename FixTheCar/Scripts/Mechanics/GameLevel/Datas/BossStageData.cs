using UnityEngine;

namespace Mechanics.GameLevel.Stages.BossStagePart
{
    [CreateAssetMenu(menuName = "Config/Stages/Boss", order = 51)]
    public class BossStageData : StageData
    {
        [Min(1)] public int HelthPlayer;
        [Min(0.1f)] public float TimeFlyHandBoss = 3;
        [Min(0.1f)] public float TimeRotateToTarget;
        [Min(0.1f)] public float DelayBetweenFirstAndSecondAttack = 1;
        [Min(0.1f)] public float TimePauseAttackMin = 2;
        [Min(0.1f)] public float TimePauseAttackMax = 4;
        
        protected override bool ValidateMethod(Stage stageTocheck)
        {
            if (TimePauseAttackMin > TimePauseAttackMax)
                TimePauseAttackMin = TimePauseAttackMax;
            return stageTocheck is BossStage;
        }
    }
}