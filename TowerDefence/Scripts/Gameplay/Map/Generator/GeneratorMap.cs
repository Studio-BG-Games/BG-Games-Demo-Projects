using System;
using System.Collections;
using System.Threading.Tasks;
using Gameplay.Map.ChunkMaster.Scripts.ChunkMaster.ChunkMaster.src.Unity.Framework;
using Gameplay.Map.ConfigData;
using Gameplay.Map.Saves;
using Plugins.DIContainer;
using Plugins.Interfaces;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Gameplay.Map.Generator
{
    public abstract class GeneratorMap
    {
        private ICoroutineRunner _coroutineRunner;
        private ICoroutineRunner CoroutineRunner => _coroutineRunner ??= (ICoroutineRunner)DiBox.MainBox.ResolveSingle(typeof(ICoroutineRunner));
        public event Action<UnitySector> SectorCreate;
        public event Action<float> Progress;
        public event Action FinishedGeneration;
        
        public void Generate(DataMap dataMap, UnityWorld unityWorld)
        {
            Init(dataMap);
            CoroutineRunner.StartCoroutine(GenerateWorld(dataMap, unityWorld));
        }

        private IEnumerator GenerateWorld(DataMap dataMap, UnityWorld unityWorld)
        {
            Vector2Int sizeMap = dataMap.MapSettings.Size;
            unityWorld.name = dataMap.Name;
            float allProgress = sizeMap.x * sizeMap.y;
            float currentProgres = 0;
            yield return GoOverSecotr(dataMap, unityWorld, sector =>
            {
                SpawnSector(sector, dataMap);
                currentProgres++;
                Progress?.Invoke(currentProgres / allProgress);
                SectorCreate?.Invoke(sector);
            });
            yield return GenerateProp(dataMap, unityWorld);
            Progress?.Invoke(1f);
            FinishedGeneration?.Invoke();
        }

        protected abstract IEnumerator GenerateProp(DataMap dataMap, UnityWorld unityWorld);

        protected IEnumerator GoOverSecotr(DataMap dataMap, UnityWorld unityWorld, Action<UnitySector> callback)
        {
            Vector2Int sizeMap = dataMap.MapSettings.Size;
            int sectorSize = unityWorld.sectorSize = dataMap.ChunkSettings.SectorSize;
            int chunkSize = unityWorld.chunkSize = dataMap.ChunkSettings.ChunkSize;
            for (int x = 0; x < sizeMap.x; x++)
            {
                for (int y = 0; y < sizeMap.y; y++)
                {
                    int startXofSector = x * sectorSize * chunkSize;
                    int startZofSector = y * sectorSize * chunkSize; //size карты идет в виде ветора по Х и У. Х остается на своем месте, Y переходит в Z, чтобы карта не была вертикальной
                    var sector = unityWorld.GetSector(new Vector3(startXofSector, 0, startZofSector));
                    callback?.Invoke(sector);
                    yield return null;
                }
            }
        }
        
        private void SpawnSector(UnitySector sector, DataMap dataMap)
        {
            int secotrSize = dataMap.ChunkSettings.SectorSize;
            int chunkSize = dataMap.ChunkSettings.ChunkSize;
            for (int x = 0; x < secotrSize; x++)
            {
                for (int z = 0; z < secotrSize; z++)
                {
                    Vector3 pointChunk = new Vector3(
                        CordPosChunk(x, sector.worldPosition.x), 
                        CordPosChunk(0, sector.worldPosition.y), 
                        CordPosChunk(z, sector.worldPosition.z));
                    SpawnChunk((UnityChunk)sector.GetChunk(pointChunk), dataMap);
                }
            }

            float CordPosChunk(int i, double cordAxis) => (float)(cordAxis + chunkSize * i);
        }

        private void SpawnChunk(UnityChunk chunk, DataMap dataMap)
        {
            int chunkSize = dataMap.ChunkSettings.ChunkSize;
            for (float x = 0; x < dataMap.ChunkSettings.ChunkSize; x++)
            {
                for (float z = 0; z < dataMap.ChunkSettings.ChunkSize; z++)
                {
                    UnityBlock block = (UnityBlock)chunk.GetBlock(
                        new Vector3(
                            chunk.worldPosition.x + x, 
                               chunk.worldPosition.y, 
                            chunk.worldPosition.z + z));
                    block.SetContent(new SaveDataBlock());
                    var realXCord = (x - (int) x) + block.worldPosition.x;
                    var realZCord = (z - (int) z) + block.worldPosition.z;
                    block.SetContent(SpawnContent(block, dataMap, realXCord, realZCord));
                }
            }
        }

        public abstract SaveDataBlock SpawnContent(UnityBlock block, DataMap dataMap, float realXCord, float realZCord);

        protected abstract void Init(DataMap dataMap);
    }
}