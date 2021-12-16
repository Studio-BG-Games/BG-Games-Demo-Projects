using Gameplay.Map.ConfigData;
using Gameplay.Map.ChunkMaster.Scripts.ChunkMaster.ChunkMaster.src.Unity.Framework;

namespace Gameplay.Map
{
    public class WorldShell
    {
        public UnityWorld World { get; }
        public DataMap DataMap { get; }

        public WorldShell(UnityWorld world, DataMap dataMap)
        {
            World = world;
            DataMap = dataMap;
        }
    }
}