using System;
using Gameplay.Map.attribute;
using Gameplay.Map.ConfigData.ModuleGenerates;
using PerlinNode;
using UnityEngine;

namespace Gameplay.Map.ConfigData
{
    [Serializable]
    public class MapSettings
    {
        public Vector2Int Size=>_size;
        [SerializeField] private Vector2Int _size;
        
        public int Seed=>_seed;
        [SerializeField] private int _seed;

        public float WaterLayer => _waterLayer;
        [SerializeField] private float _waterLayer;
        
        public HeightModule HeightModule => _heightModule;
        [Header("_______________")][SerializeField] private HeightModule _heightModule;
        
        public BiomeModule BiomeModule => _biomeModule;
        [Header("_______________")][SerializeField] private BiomeModule _biomeModule;

        public ListDefaultBlock ListDefaultBlock => _listDefaultBlock;
        [Header("_______________")][AlwaysFullDefaultBlock][SerializeField] private ListDefaultBlock _listDefaultBlock;

        public MapSettings(Vector2Int size, int seed)
        {
            if(size.x<1||size.y<1) throw new Exception("Wrong size");
            _seed = seed;
            _size = size;
        }
    }
}
