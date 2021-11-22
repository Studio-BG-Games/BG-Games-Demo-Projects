using System;
using Mechanics.GameLevel.Datas;
using Mechanics.GameLevel.Stages.СanistorStageParts.StateMAchine;
using Mechanics.Interfaces;
using UnityEngine;

namespace Mechanics.GameLevel.Stages.СanistorStageParts
{
    public class CanistorStage : Stage
    {
        public event Action<Player> GetPlayer;
        public CanistroStageData Data => _data;
        
        public override SizeElement SizeElement => _sizeElement;
        [SerializeField] private SizeElement _sizeElement;
        [SerializeField] private CanistorStageStageMachine _stateMschine;
        [SerializeField] private AbsTransitPlayer _transitPlayer;
        [SerializeField] private FactoryCanistro _factoryCanistro;
        [SerializeField] private BigCanistro _bigCanistro;

        [Header("Point")]
        [SerializeField] private Transform _cameraPoint;
        [SerializeField] private Transform _playerPoint;

        private CanistroStageData _data;

        private void Awake() => _bigCanistro.Finished += OnFinished;

        public override void StartStage(Player player, bool isInstantaneousTransit)
        {
            GetPlayer?.Invoke(player);
            _factoryCanistro.Generate();
            if(isInstantaneousTransit) _transitPlayer.InstantaneousTransit(player, _cameraPoint.position, _playerPoint.position, StartMe);
            else _transitPlayer.Transit(player, _cameraPoint.position, StartMe);
        }

        private void StartMe() => _stateMschine.StartMe();

        private void OnFinished()
        {
            _bigCanistro.Finished -= OnFinished;
            Completed?.Invoke();
        }

        public override void Init(StageData stageData)
        {
            _data = stageData as CanistroStageData;
            if (!_data) throw null;
        }
    }
}