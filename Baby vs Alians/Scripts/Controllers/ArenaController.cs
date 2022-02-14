using System;
using UnityEngine;

namespace Baby_vs_Aliens
{
    public class ArenaController : BaseController
    {
        #region Fields

        private ArenaView _view;
        private Context _context;
        private RandomArenaGenerator _generator;

        #endregion


        #region ClassLifeCycles

        public ArenaController()
        {
            _context = ServiceLocator.GetService<Context>();
            _generator = new RandomArenaGenerator();

            InstantiatePrefab<ArenaView>(_context.ArenaPrefab, null, InitView);
        }

        #endregion


        #region Methods

        private void InitView(ArenaView view)
        {
            _view = view;
        }

        public void ResetArena()
        {
            _view.ClearArena();
            _view.CreateArenaFromLevelData(_generator.CreateRandomArena(_view.Size));
        }

        public void OpenDoor()
        {
            _view.OpenDoor();
        }

        public void SetExitCallback(Action callback)
        {
            _view.ExitReachedCallback = callback;
        }

        public Vector3 GetRandomPosition()
        {
            return _view.GetRandomPosition();
        }

        public Vector3 GetPlayerSpawnPosition()
        {
            return _view.GetPlayerSpawnPosition();
        }

        public Vector3 GetEnemySpawnPosition(EnemyType type)
        {
            return _view.GetEnemySpawnPosition(type);
        }

        public void SetUpArenaByLevelIndex(int levelNumber)
        {
            if (!_context.LevelSet.IsLoadedSuccessfully ||
                levelNumber < 0 || levelNumber >= _context.LevelSet.LevelCount)
            {
                ResetArena();
                return;
            }

            _view.ClearArena();
            var level =_context.LevelSet.Levels[levelNumber];
            _view.CreateArenaFromLevelData(level.LevelData);
        }

        #endregion
    }
}