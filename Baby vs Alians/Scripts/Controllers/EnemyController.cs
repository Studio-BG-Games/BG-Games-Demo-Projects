using System;
using System.Collections.Generic;

namespace Baby_vs_Aliens
{
    public class EnemyController : BaseController, IUpdateableRegular
    {
        #region Fields

        private ArenaController _arenaController;
        private List<IEnemy> _enemies = new List<IEnemy>();
        private PlayerController _playerController;
        public event Action AllEnemiesKilled;
        private Action<int> _scoreCallback;
        private Context _context;

        public readonly SubscriptionProperty<bool> AreEnemiesAlive = new SubscriptionProperty<bool>();

        #endregion


        #region ClassLifeCycles

        public EnemyController(ArenaController arenaController, PlayerController playerController, Action<int> scoreCallback)
        {
            _arenaController = arenaController;
            _playerController = playerController;
            _scoreCallback = scoreCallback;
            _context = ServiceLocator.GetService<Context>();
        }

        #endregion


        #region Methods

        public void SpawnEnemiesDefault()
        {
            _enemies.Add(new BigEnemy(_arenaController, _playerController, _scoreCallback, EnemyType.Big));
            _enemies.Add(new BigEnemy(_arenaController, _playerController, _scoreCallback, EnemyType.Big));
            _enemies.Add(new EnemySwarm(_arenaController, _playerController, _scoreCallback, EnemyType.Small, 5));
        }

        public void SpawnEnemiesByLevelIndex(int levelNumber)
        {
            int bigEnemyAmount;
            int smallEnemyAmount;

            if (levelNumber >=0 && levelNumber < _context.LevelSet.LevelCount)
            {
                var level = _context.LevelSet.Levels[levelNumber];
                bigEnemyAmount = level.BigEnemiesAmount;
                smallEnemyAmount = level.SmallEnemyAmount;
            }
            else
            {
                var enemyAmount = _context.EnemyAmount;

                bigEnemyAmount = enemyAmount.BigEnemyAmount + levelNumber/enemyAmount.LevelsToAddBigEnemy;
                smallEnemyAmount = enemyAmount.SmallEnemyAmount + levelNumber/enemyAmount.LevelsToAddSmallEnemy;
            }

            for (int i = 0; i< bigEnemyAmount; i++)
                _enemies.Add(new BigEnemy(_arenaController, _playerController, _scoreCallback, EnemyType.Big));

            _enemies.Add(new EnemySwarm(_arenaController, _playerController, _scoreCallback, EnemyType.Small, smallEnemyAmount));
        }


        public void RemoveEnemies()
        {
            for (int i = 0; i < _enemies.Count; i++)
                _enemies[i].Dispose();
            _enemies.Clear();
        }

        #endregion


        #region IUpdatableRegular

        public void UpdateRegular()
        {
            for (int i = 0; i < _enemies.Count; i++)
                if (_enemies[i].IsDone)
                {
                    _enemies[i].Dispose();
                    _enemies.Remove(_enemies[i]);
                }
                else
                    _enemies[i].UpdateRegular();

            AreEnemiesAlive.Value = _enemies.Count > 0 ? true : false;

            if (_enemies.Count == 0)
            {
                AllEnemiesKilled?.Invoke();
            }
        }

        #endregion


        #region IDisposable

        protected override void OnDispose()
        {
            RemoveEnemies();
            base.OnDispose();
        }

        #endregion
    }
}