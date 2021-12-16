using System;
using Factorys;
using Infrastructure.SceneStates;
using Plugins.DIContainer;
using UnityEngine;

namespace Gameplay.GameSceneScript
{
    public class Gold : MonoBehaviour, IGold
    {
        [SerializeField]private int _current = 500;

        public int Current => _current;
        
        [DI] private FactoryUIForGameScene _factoryUi;
        [DI] private GameSceneData _gameSceneData;

        private void Awake()
        {
            _current = _gameSceneData.Level.StartGold;
            Update?.Invoke(_current);
        }

        public event Action<int> Update;

        public void Add(int count)
        {
            _current += count;
            Update?.Invoke(_current);
        }

        public bool TryRemove(int count)
        {
            if (count > _current)
                return false;
            _current -= count;
            Update?.Invoke(_current);
            return true;
        }
    }
}