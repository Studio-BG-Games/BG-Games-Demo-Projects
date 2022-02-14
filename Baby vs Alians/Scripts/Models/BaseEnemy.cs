using System;
using UnityEngine;

namespace Baby_vs_Aliens
{
    public abstract class BaseEnemy : IEnemy
    {
        #region Fields

        protected EnemyView _view;
        protected EnemyType _type;
        protected IHealthHolder _health;
        protected ArenaController _arenaController;
        protected PlayerController _playerController;
        protected EnemyConfig _config;
        protected HealthBar _healthBar;

        protected EnemyState _currentState;
        protected EnemyState _moveState;
        protected EnemyState _attackState;
        protected EnemyState _deathState;

        protected Action<int> _scoreUpdateCallback;

        protected float _healthBarHideTimer = 2f;

        #endregion


        #region Properties

        public bool IsDone => _currentState.IsDone;
        public EnemyView View => _view;
        public EnemyConfig Config => _config;
        public ArenaController ArenaController => _arenaController;
        public Vector3 TargetPosition => _playerController.PlayerPosition;
        public bool IsTargetAlive => _playerController.IsPlayerAlive;

        #endregion


        #region ClassLifeCycles

        public BaseEnemy(ArenaController arenaController, PlayerController playerController, Action<int>scoreCallback, EnemyType type)
        {
            _arenaController = arenaController;
            _playerController = playerController;
            _type = type;
            _scoreUpdateCallback = scoreCallback;

            _view = ServiceLocator.GetService<ObjectPoolManager>().
                GetEnemyPool(_type).Pop().GetComponent<EnemyView>();

            _config = _view.Config;

            _view.InitAtPosition(_arenaController.GetEnemySpawnPosition(_type));

            _health = new HealthHolder(_config.Health);

            _view.Damaged += _health.TakeDamage;
            _health.Death += KillEnemy;
            _health.Damaged += OnDamaged;

            _healthBar = _view.GetComponentInChildren<HealthBar>();

            if (_healthBar)
                _health.HealthPercentage.SubscribeOnChange(UpdateHealthBar);

            InitStates();
        }

        #endregion


        #region Methods

        protected abstract void InitStates();

        protected void KillEnemy()
        {
            if (_currentState == _deathState)
                return;

            _scoreUpdateCallback.Invoke(_config.ScoreValue);
            _view.Die();
            SetDeathState();
        }

        protected void UpdateHealthBar(float percentage)
        {
            _healthBar.SetBarSize(percentage);
            _healthBar.SetToDisable(_healthBarHideTimer);
        }

        protected void OnDamaged()
        {
            _view.OnDamaged();
        }

        public void SetAttackState() => _currentState = _attackState;
        public void SetMoveState() => _currentState = _moveState;
        public void SetDeathState() => _currentState = _deathState;

        #endregion


        #region IUpdateableRegular

        public void UpdateRegular()
        {
            _currentState.UpdateRegular();
        }

        #endregion


        #region IDisposeable

        public void Dispose()
        {
            _view.Damaged -= _health.TakeDamage;
            _health.Death -= KillEnemy;
            _health.Damaged -= OnDamaged;
            _health.HealthPercentage.UnSubscribeOnChange(UpdateHealthBar);
            _healthBar?.Disable();

            if (_view == null)
                return;

            ServiceLocator.GetService<ObjectPoolManager>().
                GetEnemyPool(_type).Push(_view.gameObject);
        }

        #endregion
    }
}