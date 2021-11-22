using System;
using Mechanics.GameLevel.Datas;
using Mechanics.GameLevel.Stages.BossStagePart.Interfaces;
using Mechanics.GameLevel.Stages.ElectroStageParts.Machines;
using Mechanics.GameLevel.Stages.ElectroStageParts.Wire;
using Mechanics.Interfaces;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Mechanics.GameLevel.Stages.ElectroStageParts
{
    public class ElectroStage : Stage
    {
        public override SizeElement SizeElement => _sizeElement;
        public int MaxBreakeWires => _wires.MaxBreakePart;

        [SerializeField] private SizeElement _sizeElement;
        [SerializeField] private ElectroStageStateMachine _stateMachine;
        [SerializeField] private Wires _wires;
        [SerializeField] private ElectroPath electroPath;
        [SerializeField] private AbsTransitPlayer _transitPlayer;
        [SerializeField] private Transform _pointCamera;
        [SerializeField] private Transform _pointPlayer;
        
        private ElectroStageData _stageData;

        public override void StartStage(Player player, bool isInstantaneousTrnasit)
        {
            foreach (var initPlayer in GetComponentsInChildren<IInitPlayer>()) initPlayer.Init(player);
            _wires.Break(Random.Range(_stageData.MinBreakWires, _stageData.MaxBreakeWires+1));
            if (isInstantaneousTrnasit) InstantaneousTransit(player);
            else NormalTransit(player);
        }

        private void NormalTransit(Player player) => _transitPlayer.Transit(player, _pointCamera.position, StartStage);

        private void InstantaneousTransit(Player player) 
            => _transitPlayer.InstantaneousTransit(player, _pointCamera.position, _pointPlayer.position, () => StartStage());

        private void StartStage()
        {
            _stateMachine.StartMe();
            electroPath.Finished += OnFinished;
        }

        private void OnFinished()
        {
            electroPath.Finished -= OnFinished;
            Completed?.Invoke();
        }

        public override void Init(StageData stageData)
        {
            _stageData = stageData as ElectroStageData;
            if(!_stageData)
                throw new Exception("Wrong Data");
        }
    }
}