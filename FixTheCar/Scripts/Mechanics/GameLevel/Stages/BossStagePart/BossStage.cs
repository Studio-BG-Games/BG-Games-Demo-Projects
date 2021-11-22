using System;
using Mechanics.GameLevel.Stages.BossStagePart.Interfaces;
using Mechanics.GameLevel.Stages.BossStagePart.StateMachine;
using Mechanics.Interfaces;
using Mechanics.TransitPlayer;
using UnityEngine;

namespace Mechanics.GameLevel.Stages.BossStagePart
{
    public class BossStage : Stage
    {
        public event Action<Player> PlayerWasGet;
        
        public override SizeElement SizeElement => _size;
        [SerializeField] private SizeElement _size;
        [SerializeField] private Transform _pointCamera;
        [SerializeField] private Transform _pointPlayer;
        [SerializeField] private AbsTransitPlayer _playerTransit;
        [SerializeField] private BossStageStateMachine _stateMachine;
        [SerializeField] private Boss _boss;
        [SerializeField] private PlayerHelth _playerHelth;
        
        private BossStageData _data;
        private Player _player;
        
        public override void StartStage(Player player, bool isInstantaneousTransit)
        {
            _player = player;
            PlayerWasGet?.Invoke(player);
            if(isInstantaneousTransit) _playerTransit.InstantaneousTransit(player, _pointCamera.position, _pointPlayer.position, StartMe);
            else _playerTransit.Transit(player, _pointCamera.position, StartMe);
        }

        private void StartMe()
        {
            foreach (IInitBossStageData child in GetComponentsInChildren<IInitBossStageData>()) child.Init(_data);
            foreach (var child in GetComponentsInChildren<IInitPlayerHealth>()) child.Init(_playerHelth);
            _boss.Finished += OnFinished;
            _stateMachine.StartMe();
        }

        public void Restart(Action callback = null)
        {
            foreach (var restartable in GetComponentsInChildren<IRestartable>()) restartable.Restart();
            _player.transform.position = _pointPlayer.position;
            callback?.Invoke();
        }
        
        private void OnFinished() => Completed?.Invoke();

        public override void Init(StageData stageData)
        {
            _data = stageData as BossStageData;
            if (!_data) throw null;
        }
    }
}