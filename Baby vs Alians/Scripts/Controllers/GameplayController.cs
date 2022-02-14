using System.Collections.Generic;
using UnityEngine;

namespace Baby_vs_Aliens
{
    public class GameplayController : BaseController, IUpdateableRegular
    {
        #region Fields

        private List<IUpdateableRegular> _updateablesRegular;
        private bool _isGameActive;
        private InputController _inputController;
        private GameplayUIController _gameplayUiController;
        private EnemyController _enemyController;
        private PlayerController _playerController;
        private ArenaController _arenaController;
        private PlayerProfile _playerProfile;
        private Context _context;
        private PlayerFactory _factory;

        #endregion


        #region ClassLifeCycles

        public GameplayController(PlayerProfile playerProfile)
        {
            Camera.main.orthographic = true;

            _context = ServiceLocator.GetService<Context>();
            _playerProfile = playerProfile;
            _updateablesRegular = new List<IUpdateableRegular>();

            _factory = new PlayerFactory(_context);

            _inputController = new InputController();
            AddController(_inputController);
            _updateablesRegular.Add(_inputController);

            _gameplayUiController = new GameplayUIController(playerProfile, _inputController);
            AddController(_gameplayUiController);
            _updateablesRegular.Add(_gameplayUiController);

            _arenaController = new ArenaController();
            AddController(_arenaController);

            _arenaController.SetUpArenaByLevelIndex(_playerProfile.CurrentLevel);

            _playerController = new PlayerController(_inputController, _arenaController,
                _gameplayUiController, _playerProfile.CustomizationInfo, _factory);
            AddController(_playerController);
            _updateablesRegular.Add(_playerController);

            _enemyController = new EnemyController(_arenaController, _playerController, _gameplayUiController.UpdateScore);
            AddController(_enemyController);
            _updateablesRegular.Add(_enemyController);

            _enemyController.AreEnemiesAlive.SubscribeOnChange(_playerController.GetEnemyPresence);

            if (_context.LevelSet.IsLoadedSuccessfully)
                _enemyController.SpawnEnemiesByLevelIndex(_playerProfile.CurrentLevel);
            else
                _enemyController.SpawnEnemiesDefault();

            _gameplayUiController.BeginFadeIn(OnFadeIn);

            
            _arenaController.SetExitCallback(NextLevel);
            _enemyController.AllEnemiesKilled += _arenaController.OpenDoor;
            _playerController.OutOfLives += GameOver;
        }

        #endregion


        #region IUpdateableRegular

        public void UpdateRegular()
        {
            if (_isGameActive)
            {
                for (int i = 0; i < _updateablesRegular.Count; i++)
                {
                    _updateablesRegular[i].UpdateRegular();
                }
            }
            else
            {
                (_gameplayUiController as IUpdateableRegular).UpdateRegular();
            }
        }

        #endregion


        #region Methods

        private void OnFadeIn()
        {
            _isGameActive = true;
        }

        private void OnFadeOut() 
        {

            _arenaController.SetUpArenaByLevelIndex(_playerProfile.CurrentLevel);
            _enemyController.SpawnEnemiesByLevelIndex(_playerProfile.CurrentLevel);

            _playerController.RespawnPlayer();
            _gameplayUiController.BeginFadeIn(OnFadeIn);
        }

        private void NextLevel()
        {
            _isGameActive = false;
            _gameplayUiController.BeginFadeOut(OnFadeOut);
            if (_context.AreLevelsLooped)
                _playerProfile.CurrentLevel = _playerProfile.CurrentLevel + 1 < _context.LevelSet.LevelCount ?
                    _playerProfile.CurrentLevel + 1 : 0;
            else
                _playerProfile.CurrentLevel++;
        }

        protected override void OnDispose()
        {
            _enemyController.AllEnemiesKilled -= _arenaController.OpenDoor;
            _enemyController.AllEnemiesKilled -= _arenaController.OpenDoor;
            _updateablesRegular.Clear();
            _factory.Dispose();
            base.OnDispose();
        }

        private void GameOver()
        {
            _gameplayUiController.BeginFadeOut(() => { _playerProfile.CurrentState.Value = GameState.Menu; });
        }

        #endregion
    }
}