using Mechanics.GameLevel.Stages.BossStagePart.Interfaces;
using Plugins.DIContainer;
using Services.IInputs;
using Services.Interfaces;
using UnityEngine;

namespace Mechanics.GameLevel.Stages.BossStagePart.StateMachine.State
{
    public class FightWithBoss : StageBossState, IInitPlayerHealth
    {
        [SerializeField] private Boss _boss;
        [SerializeField] private AgregatorPointerReciver _agregatorPointerReciver;
        [SerializeField] private BossStage _bossStage;
        [SerializeField] private WinFightWithBoss _winFightWithBoss;
        [SerializeField] private LoseFightWithBoss _loseFightWithBoss;

        private Player _player;
        private StageBossState _stateToTransit;
        private PlayerHelth _playerHealth;

        [DI] private IInput _input;

        [DI]
        private void Init() => _bossStage.PlayerWasGet += OnPlayerGet;

        public override void On()
        {
            _boss.StartMe();
            _playerHealth.HealthUpdate += OnHealthUpdate;
            _agregatorPointerReciver.OnAll();
            _player.ChangeActiveMover(true);
            _boss.Finished += OnFinished;
            (_input as IShowHide)?.Show();
        }

        private void OnHealthUpdate(int obj)
        {
            if (obj == 0) _stateToTransit = _loseFightWithBoss;
        }

        public override void Off()
        {
            _boss.Finished -= OnFinished;
            _boss.Restart();
            _agregatorPointerReciver.OffAll();
            _player?.ChangeActiveMover(false);
            if(_playerHealth)
                _playerHealth.HealthUpdate += OnHealthUpdate;
            _stateToTransit = null;
            (_input as IShowHide)?.Hide();
        }

        public override Stages.State TransitToOrNull() => _stateToTransit;

        private void OnFinished() => _stateToTransit = _winFightWithBoss;

        private void OnPlayerGet(Player player)
        {
            _bossStage.PlayerWasGet -= OnPlayerGet;
            _player = player;
        }

        public void Init(PlayerHelth health) => _playerHealth = health;
    }
}