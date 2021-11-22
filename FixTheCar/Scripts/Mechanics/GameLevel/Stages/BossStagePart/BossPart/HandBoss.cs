using System;
using System.Globalization;
using Mechanics.GameLevel.Stages.BossStagePart.Interfaces;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Mechanics.GameLevel.Stages.BossStagePart
{
    public class HandBoss : MonoBehaviour, IInitBossStageData
    {
        [SerializeField] private Stick _stick;
        [SerializeField] private Transform[] _targets;
        private BossStageData _data;

        private Transform RandomTarget => _targets[Random.Range(0, _targets.Length)];
        
        public void ToAttack()
        {
            var target = RandomTarget;
            _stick.Rotate(target, _data.TimeRotateToTarget, () => _stick.LongTo(target, _data.TimeFlyHandBoss));
        }

        public void ToIdel() => _stick.MakeShort(_data.TimeFlyHandBoss, () => _stick.RotateToZero(_data.TimeRotateToTarget));

        public void ToIdel(float durationFly, float durationRotate) => _stick.MakeShort(durationFly, () => _stick.RotateToZero(durationRotate));
        
        public void Init(BossStageData data) => _data = data;
    }
}