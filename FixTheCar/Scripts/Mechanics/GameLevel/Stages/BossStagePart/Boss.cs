using System;
using System.Collections;
using System.Collections.Generic;
using Mechanics.GameLevel.Stages.BossStagePart.Interfaces;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Mechanics.GameLevel.Stages.BossStagePart
{
    public class Boss : MonoBehaviour, IInitBossStageData, IRestartable
    {
        public event Action Finished;

        [SerializeField] private List<HandBoss> _handBoss;

        private IBossPart[] _bossParts;
        private BossStageData _data;
        private Coroutine _logic;

        private void Awake() => InitIBossPart();

        public void StartMe() => _logic = StartCoroutine(Logic());

        public void Restart()
        {
            _handBoss.ForEach(x=>x.ToIdel(0.0001f, 0.00001f));
            if(_logic!=null) StopCoroutine(_logic);
        }

        public void Init(BossStageData data) => _data = data;

        private void InitIBossPart()
        {
            _bossParts = GetComponentsInChildren<IBossPart>();
            foreach (IBossPart bossPart in _bossParts) bossPart.Cleaned += OnCleanedPart;
        }

        private void OnCleanedPart()
        {
            foreach (var bossPart in _bossParts) if (!bossPart.IsClear()) return;
            _handBoss.ForEach(x=>x.ToIdel(_data.TimeFlyHandBoss, _data.TimeRotateToTarget));
            Finished?.Invoke();
        }

        private IEnumerator Logic()
        {
            while (true)
            {
                _handBoss[0].ToAttack();
                yield return new WaitForSeconds(_data.TimeFlyHandBoss+1);
                _handBoss[1].ToAttack();
                yield return new WaitForSeconds(_data.TimeFlyHandBoss+1);
                _handBoss[0].ToIdel();
                _handBoss[1].ToIdel();
                yield return new WaitForSeconds(_data.TimeFlyHandBoss+Random.Range(_data.TimePauseAttackMin, _data.TimePauseAttackMax));
            }
        }
    }
}