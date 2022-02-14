using System;
using UnityEngine;

namespace Baby_vs_Aliens
{
    [Serializable]
    public class Level
    {
        private int _width = 21;
        private int _height = 13;
        private int _bigEnemiesAmount;
        private int _smallEnemiesAmount;

        [SerializeField] private ArenaObjectType[] _serializedLevelData;

        public ArenaObjectType[,] LevelData;
        public int BigEnemiesAmount => _bigEnemiesAmount;
        public int SmallEnemyAmount => _smallEnemiesAmount;

        public int Width => _width;
        public int Height => _height;

        public ArenaObjectType[] SerializedLevelData => _serializedLevelData;

        public Level()
        {
            CreateLevelData();
        }

        public Level(int width, int height)
        {
            _width = width;
            _height = height;
            CreateLevelData();
        }

        private void CreateLevelData()
        {
            LevelData = new ArenaObjectType[_width, _height];
        }

        public void ConvertToSerializable()
        {
            var i = 0;
            _serializedLevelData = new ArenaObjectType[_width * _height];
            for (int y = 0; y < _height; y++)
                for (var x = 0; x < _width; x++)
                    _serializedLevelData[i++] = LevelData[x, y];
        }

        public void ConvertFromSerializable()
        {
            LevelData = new ArenaObjectType[_width, _height];

            _bigEnemiesAmount = 0;
            _smallEnemiesAmount = 0;

            for (var x = 0; x < _width; x++)
                for (var y = 0; y < _height; y++)
                {
                    LevelData[x, y] = _serializedLevelData[y * _width + x];
                    if (LevelData[x, y] == ArenaObjectType.BigEnemySpawn)
                        _bigEnemiesAmount++;
                    if (LevelData[x, y] == ArenaObjectType.SmallEnemySpawn)
                        _smallEnemiesAmount++;
                }
        }

        public void CopySerializedDataFromExisting(ArenaObjectType[] serializedLevelData)
        {
            _serializedLevelData = new ArenaObjectType[serializedLevelData.Length];
            for (int i = 0; i < serializedLevelData.Length; i++)
                _serializedLevelData[i] = serializedLevelData[i];
        }
    }
}