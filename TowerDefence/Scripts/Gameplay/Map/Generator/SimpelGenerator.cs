using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Factorys;
using Gameplay.Builds.Data;
using Gameplay.Map.ChunkMaster.Scripts.ChunkMaster.ChunkMaster.src.Unity.Framework;
using Gameplay.Map.ChunkMaster.Scripts.ChunkMaster.ChunkMaster.src.Utils;
using Gameplay.Map.ConfigData;
using Gameplay.Map.Saves;
using LibNoise;
using Plugins.DIContainer;
using Plugins.Interfaces;
using Unity.Mathematics;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Gameplay.Map.Generator
{
    [Serializable]
    public class SimpelGenerator : GeneratorMap
    {
        private bool IsFlat = true;
        public bool IsRandomSeed;
        public bool WithWater;
        private int SeedLandscape;
        private int SeedPropBuilds;

        public SimpelGenerator(int seedLandscape, int seedPropBuilds, bool withWater, bool isRandomSeed = false)
        {
            SeedLandscape = seedLandscape;
            IsRandomSeed = isRandomSeed;
            WithWater = withWater;
            IsFlat = true;
            SeedPropBuilds = seedPropBuilds;
        }
        
        private FactoryBuild FactoryBuild=> _factoryBuild==null? _factoryBuild = DiBox.MainBox.ResolveSingle<FactoryBuild>() : _factoryBuild;
        private FactoryBuild _factoryBuild;

        public override SaveDataBlock SpawnContent(UnityBlock block, DataMap dataMap, float realXCord, float realZCord)
        {
            SaveDataBlock dataBlockBlock = block.GetContent();
            Biome biome = dataMap.MapSettings.BiomeModule.Get(realXCord, realZCord);
            var heightBrick = GetHeight(dataMap, realXCord, realZCord);
            Brick templateBrick = GetTemplateBlock(dataMap, biome, heightBrick, out bool isWater);

            var positionSpawn = GetPositonBlockInWorld(realXCord, realZCord, heightBrick, dataMap);
            var newBrickInScene = Object.Instantiate(templateBrick, positionSpawn, quaternion.identity, block.gameObject.transform);
            newBrickInScene.Init(biome, isWater);
            dataBlockBlock.AddBrick(newBrickInScene);

            FillEmptyBlock(dataBlockBlock, biome, dataMap, realXCord, realZCord, positionSpawn, block);

            var leftH = GetHeight(dataMap, realXCord - 1, realZCord);
            var rightH = GetHeight(dataMap, realXCord + 1, realZCord);
            var frontH = GetHeight(dataMap, realXCord, realZCord + 1);
            var backH = GetHeight(dataMap, realXCord, realZCord - 1);
            dataBlockBlock.GoOverBrick(x =>
            {
                x.UpdateMeshByHeight(frontH, backH, leftH, rightH);
            });
            
            return dataBlockBlock;
        }

        protected override IEnumerator GenerateProp(DataMap dataMap, UnityWorld unityWorld)
        {
            System.Random rnd = new System.Random(SeedPropBuilds);
            yield return GoOverSecotr(dataMap, unityWorld, sector =>
            {
                for (int i = 0; i < dataMap.ChunkSettings.CountTryToSetPropInSector; i++)
                {
                    var posSector = sector.worldPosition;
                    var xChunk = posSector.x + rnd.Next(0, dataMap.ChunkSettings.SectorSize) * dataMap.ChunkSettings.ChunkSize;
                    var zChunk = posSector.z + rnd.Next(0, dataMap.ChunkSettings.SectorSize) * dataMap.ChunkSettings.ChunkSize;
                    var chunk = sector.GetChunk(new Vector3(xChunk, 0, zChunk));
                    var xBlock = chunk.worldPosition.x + rnd.Next(0, dataMap.ChunkSettings.ChunkSize);
                    var zBlock = chunk.worldPosition.z + rnd.Next(0, dataMap.ChunkSettings.ChunkSize);
                    var brick = chunk.GetBlock(new Point(xBlock, 0, zBlock)).GetContent().GetUpestBrick();
                    var prop = brick.BiomeBlock.GetRandomPropOrNull(rnd);
                    if(prop==null)
                        continue;
                    var yAngel = GetRandomYAngel(rnd);
                    FactoryBuild.Spawn(prop, brick.transform.position+Vector3.up/2-SizeOnMap.SingModifacateXZ(yAngel)/2, new Vector3(0, yAngel, 0));
                }
            });
        }

        private float GetRandomYAngel(System.Random rnd)
        {
            switch (rnd.Next(0,4))
            {
                case 0:
                    return -90;
                case 1:
                    return 0;
                case 2:
                    return 90;
                case 3:
                    return 180;
            }

            return 0;
        }

        private void FillEmptyBlock(SaveDataBlock dataBlockBlock, Biome biome, DataMap dataMap, float realXCord, float realZCord, Vector3 positionBlock, UnityBlock block)
        {
            List<Vector3> heights = GetPosOfNeigrbhood(dataMap, realXCord, realZCord);
            var minH = heights.OrderBy(x => x.y).First();
            if(minH.y>positionBlock.y)
                return;
            Brick underGroundBrick = GetUnderBrick(biome, dataMap);
            positionBlock.y--;
            while (minH.y<positionBlock.y)
            {
                var brick = Object.Instantiate(underGroundBrick, positionBlock, Quaternion.identity, block.gameObject.transform);
                dataBlockBlock.AddBrick(brick);
                positionBlock.y--;
            }
        }

        private List<Vector3> GetPosOfNeigrbhood(DataMap dataMap, float realXCord, float realZCord)
        {
            List<Vector3> result = new List<Vector3>();
            result.Add(GetPositonBlockInWorld(realXCord + 1, realZCord, GetHeight(dataMap, realXCord + 1, realZCord), dataMap));
            result.Add(GetPositonBlockInWorld(realXCord - 1, realZCord, GetHeight(dataMap, realXCord - 1, realZCord), dataMap));
            result.Add(GetPositonBlockInWorld(realXCord, realZCord + 1, GetHeight(dataMap, realXCord, realZCord + 1), dataMap));
            result.Add(GetPositonBlockInWorld(realXCord, realZCord - 1, GetHeight(dataMap, realXCord, realZCord - 1), dataMap));
            return result;
        }

        private Brick GetUnderBrick(Biome biome, DataMap dataMap)
        {
             var result =biome.GetDefaultBlockBiomeOrNull(DefaultBrick.Types.underground);
             if (result) return result;
             return dataMap.MapSettings.ListDefaultBlock.GetOrNull(DefaultBrick.Types.underground);
        }


        private Vector3 GetPositonBlockInWorld(float realXCord, float realZCord, float heightBrick, DataMap dataMap)
        {
            Vector3 positionSpawn = new Vector3(realXCord, heightBrick, realZCord);
            if(heightBrick < dataMap.MapSettings.WaterLayer && WithWater)
                positionSpawn = new Vector3(realXCord, dataMap.MapSettings.WaterLayer, realZCord);
            else
                positionSpawn = new Vector3(realXCord, heightBrick, realZCord);
            return positionSpawn;
        }

        private Brick GetTemplateBlock(DataMap dataMap, Biome biome, float heightBrick, out bool isWater)
        {
            isWater = false;
            if (heightBrick <= dataMap.MapSettings.WaterLayer && WithWater)
            {
                isWater = true;
                var result = biome.GetDefaultBlockBiomeOrNull(DefaultBrick.Types.Water);
                if (result)
                    return result;
                return dataMap.MapSettings.ListDefaultBlock.GetOrNull(DefaultBrick.Types.Water);
            }
            return biome.MainBrickBiome;
        }

        private float GetHeight(DataMap dataMap, float realXCord, float realZCord)
        {
            float heightBrick = dataMap.MapSettings.HeightModule.Get(realXCord, realZCord);
            if (IsFlat)
                heightBrick = Mathf.Round(heightBrick);
            return heightBrick;
        }

        protected override void Init(DataMap dataMap)
        {
            var seed = IsRandomSeed ? Random.Range(int.MinValue, int.MaxValue) : SeedLandscape;
            Debug.Log($"Seed of map + {seed}");
            dataMap.MapSettings.BiomeModule.SetSeed(seed);
            dataMap.MapSettings.HeightModule.SetSeed(seed);
        }
    }
}