using System;
using UnityEngine;

namespace Baby_vs_Aliens
{
    public class PlayerController : BaseController, IUpdateableRegular
    {
        #region Fields

        private PlayerView _view;
        private PlayerConfig _config;
        private InputController _inputController;
        private ArenaController _arenaController;
        private Context _context;
        private IHealthHolder _health;
        private CharacterCustomizationInfo _customizationInfo;
        private PlayerFactory _factory;

        private readonly SubscriptionProperty<int> _lives = new SubscriptionProperty<int>();

        public event Action OutOfLives;

        private PlayerState _currentState;
        private PlayerState _moveState;
        private PlayerState _attackState;
        private PlayerState _deathState;
        private PlayerState _damagedState;

        #endregion


        #region Properties

        public Vector3 PlayerPosition => _view.transform.position;

        public bool IsPlayerAlive => !_health.IsDead;

        public PlayerView View => _view;

        public PlayerConfig Config => _config;

        public bool AreEnemiesPresent { get; private set; }

        #endregion


        #region ClassLifeCycles

        public PlayerController(InputController inputController, ArenaController arenaController,
            GameplayUIController uiController, CharacterCustomizationInfo customizationInfo, PlayerFactory factory)
        {
            _factory = factory;
            _arenaController = arenaController;
            _context = ServiceLocator.GetService<Context>();

            _config = _context.PlayerConfig;
            _customizationInfo = customizationInfo;

            _inputController = inputController;

            _health = new HealthHolder(_config.MaxHealth);
            _lives.Value = _config.MaxLives;

            InitView(_factory.GetCharacter(_customizationInfo));

            InitStates();

            _inputController.MousePosition.SubscribeOnChange(RotatePlayer);

            _health.Death += KillPlayer;
            _health.Damaged += SetDamagedState;

            _health.HealthPercentage.SubscribeOnChange(_view.HealthBar.SetBarSize);
            uiController.RegisterHealthAndLivesTracking(_lives,_health.HealthPercentage);
        }

        #endregion


        #region Methods

        private void InitStates()
        {
            _moveState = new PlayerMoveState(this, SetAttackState);
            _attackState = new PlayerAttackState(this, SetMoveState);
            _deathState = new PlayerDeathState(this, RespawnPlayer);
            _damagedState = new PlayerDamagedState(this, SetMoveState);

            _inputController.MovementVector.SubscribeOnChange((_moveState as PlayerMoveState).SetMoveVector);
            _inputController.MovementVector.SubscribeOnChange((_attackState as PlayerAttackState).CheckForMovement);

            SetMoveState();
        }

        private void InitView(PlayerView view)
        {
            _view = view;
            _view.Damaged += _health.TakeDamage;
            _view.transform.position = _arenaController.GetPlayerSpawnPosition();

            AddGameObject(_view.gameObject);

        }

        private void RotatePlayer(Vector3 mousePos)
        {
            var lookDirection = mousePos - _view.transform.position;

            Quaternion rotation = _view.transform.rotation;
            rotation.SetLookRotation(lookDirection);
            _view.transform.rotation = rotation;
        }

        private void KillPlayer()
        {
            _lives.Value--;

            SetDeathState();
        }

        public void RespawnPlayer()
        {
            if (_lives.Value <= 0)
                OutOfLives?.Invoke();
            else
            { 
                _view.transform.position = _arenaController.GetPlayerSpawnPosition();
                _health.ResetHealth();
                _view.ShowCharacter();
                SetMoveState();
            }
        }

        public void UpdateRegular()
        {
            _currentState.UpdateRegular();
        }

        private void SetMoveState()
        {
            if (_currentState != _moveState)
            {
                _currentState = _moveState;
                _currentState.Reset();
            }
        }

        private void SetAttackState()
        {
            if (_currentState != _attackState)
            {
                _currentState = _attackState;
                _currentState.Reset();
            }
        }

        private void SetDeathState()
        {
            if (_currentState != _deathState)
            {
                _currentState = _deathState;
                _currentState.Reset();
            }
        }

        private void SetDamagedState()
        {
            if (_currentState != _damagedState)
            {
                _currentState = _damagedState;
                _currentState.Reset();
            }
        }

        public void GetEnemyPresence(bool areEnemiesPresent)
        {
            AreEnemiesPresent = areEnemiesPresent;
        }

        protected override void OnDispose()
        {
            _inputController.MovementVector.UnSubscribeOnChange((_moveState as PlayerMoveState).SetMoveVector);
            _inputController.MovementVector.UnSubscribeOnChange((_attackState as PlayerAttackState).CheckForMovement);
            _inputController.MousePosition.UnSubscribeOnChange(RotatePlayer);
            _view.Damaged -= _health.TakeDamage;
            _health.Death -= KillPlayer;
            _health.Damaged -= SetDamagedState;
            base.OnDispose();
        }

        #endregion
    }
}