using System;
using System.Collections.Generic;
using UnityEngine;

namespace Baby_vs_Aliens
{
    public class EnemySwarm : IEnemy, ISwarm
    {
        #region Fields

        private EnemyType _type;
        private ArenaController _arenaController;
        private PlayerController _playerController;
        private List<ISwarmMember> _enemies = new List<ISwarmMember>();
        private int _swarmSize;

        protected Action<int> _scoreUpdateCallback;

        #endregion


        #region Properties

        public bool IsDone => _enemies.Count == 0;

        public Vector3 LeaderPosition => _enemies[0].Position;

        #endregion


        #region ClassLifeCycles

        public EnemySwarm(ArenaController arenaController, PlayerController playerController, Action<int> scoreCallback, EnemyType type, int swarmSize)
        {
            _arenaController = arenaController;
            _playerController = playerController;
            _type = type;
            _swarmSize = swarmSize;
            _scoreUpdateCallback = scoreCallback;

            SpawnEnemies(_swarmSize);
        }

        #endregion


        #region Methods

        private void SpawnEnemies(int amount)
        {
            for (int i =0; i < amount; i++)
            {
                var enemy = new SwarmMember(this, (i == 0 ? true : false), _arenaController, _playerController, _scoreUpdateCallback, _type);
                _enemies.Add(enemy);
            }
        }

        #endregion


        #region IUpdateableRegular

        public void UpdateRegular()
        {
            for (int i = _enemies.Count - 1; i >= 0; i--)
            {
                var enemy = _enemies[i];
                if (!enemy.IsDone)
                    enemy.UpdateRegular();
                else
                {
                    enemy.Dispose();
                    _enemies.Remove(enemy);
                    if (i == 0 && _enemies.Count > 0)
                        _enemies[i].IsSwarmLeader = true;
                }
                
            }
        }

        #endregion


        #region IDisposeable

        public void Dispose()
        {
            foreach (var enemy in _enemies)
                enemy.Dispose();
            _enemies.Clear();
        }

        #endregion
    }
}