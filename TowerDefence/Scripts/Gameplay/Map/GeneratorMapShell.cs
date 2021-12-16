using System;
using Gameplay.Map.ChunkMaster.Scripts.ChunkMaster.ChunkMaster.src.Unity.Framework;
using Gameplay.Map.ConfigData;
using Gameplay.Map.Generator;
using Plugins.DIContainer;
using UnityEngine;

namespace Gameplay.Map
{
    public class GeneratorMapShell : MonoBehaviour
    {
        [DI] private DataMap _dataMap;

        [SerializeField] private SimpelGenerator _simpelGenerator;
        
        private void Awake()
        {
            Debug.Log(_dataMap);
            var parent = new GameObject("Map parent");
            var world = new UnityWorld(parent.transform, _dataMap.Name, _dataMap.ChunkSettings.ChunkSize, _dataMap.ChunkSettings.SectorSize);
            _simpelGenerator.Generate(_dataMap, world);
        }
    }
}