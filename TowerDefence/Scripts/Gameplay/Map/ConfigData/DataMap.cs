using System;
using UnityEngine;

namespace Gameplay.Map.ConfigData
{
    [Serializable]
    public class DataMap
    {
        public SectorSeetings ChunkSettings => _chunkSettings;
        [SerializeField] private SectorSeetings _chunkSettings;
        [Header("_______________")]
        [SerializeField] private MapSettings _mapSettings;

        public MapSettings MapSettings => _mapSettings;

        public string Name => _name;
        [SerializeField] private string _name;

        public DataMap(SectorSeetings chunkSettings, MapSettings mapSettings, string name)
        {
            _chunkSettings = chunkSettings;
            _mapSettings = mapSettings;
            _name = name;
        }
    }
}