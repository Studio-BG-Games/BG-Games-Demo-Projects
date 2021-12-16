using System.Collections.Generic;
using DefaultNamespace.Infrastructure.Data;
using Gameplay.Builds;
using Gameplay.Map.ConfigData;
using Gameplay.Map.Generator;
using Gameplay.Units;
using Gameplay.Waves;

namespace Infrastructure.SceneStates
{
    public class GameSceneData
    {
        public DataMap DataMap { get; }
        public GeneratorMap GeneratorMap { get; }
        public List<Build> Build { get; }
        public List<Unit> PlayerUnits { get; }
        public Level Level { get; }
        public LevelSetProfile LevelSetProfile { get; }

        public GameSceneData(DataMap dataMap, GeneratorMap generatorMap, List<Build> builds, List<Unit> units, Level level, LevelSetProfile levelSetProfile)
        {
            DataMap = dataMap;
            GeneratorMap = generatorMap;
            Build = builds;
            PlayerUnits = units;
            Level = level;
            LevelSetProfile = levelSetProfile;
        } 
    }
}