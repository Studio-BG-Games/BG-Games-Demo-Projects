using System;
using UnityEngine;

namespace Gameplay.Map.ConfigData
{
    [Serializable]
    public class SectorSeetings
    {
        [Min(1)][SerializeField] private int _sectorSize;
        [Min(5)][SerializeField] private int _chunkSize;
        [Min(0)][SerializeField] private int _countTryToSetPropInSector;

        public int CountTryToSetPropInSector => _countTryToSetPropInSector;
        public int SectorSize => _sectorSize;
        public int ChunkSize => _chunkSize;

        public SectorSeetings(int sectorSize, int chunkSize)
        {
            if(sectorSize<1) throw new Exception("SectorSize < 2");
            if(chunkSize<1) throw new Exception("chunkSize < 2");
            _sectorSize = sectorSize;
            _chunkSize = chunkSize;
        }
    }
}