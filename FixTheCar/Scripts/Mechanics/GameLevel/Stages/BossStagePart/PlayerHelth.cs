using System;
using Mechanics.GameLevel.Stages.BossStagePart.Interfaces;
using UnityEngine;

namespace Mechanics.GameLevel.Stages.BossStagePart
{
    public class PlayerHelth : MonoBehaviour, IInitBossStageData, IRestartable
    {
        public event Action<int> HealthUpdate;
        
        private int _current;
        private BossStageData _data;

        public void Init(BossStageData data)
        {
            _data = data;
            _current = data.HelthPlayer;
            ManualUpdate();
        }

        public void Restart()
        {
            _current = _data.HelthPlayer;
            ManualUpdate();
        }

        public void Damage()
        {
            _current--;
            ManualUpdate();
        }

        public void ManualUpdate() => HealthUpdate?.Invoke(_current);
    }
}