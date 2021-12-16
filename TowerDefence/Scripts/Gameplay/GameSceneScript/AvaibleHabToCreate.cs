﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Factorys;
 using Gameplay.Builds;
 using Gameplay.Builds.Data;
 using Gameplay.Builds.Data.Marks;
 using Gameplay.HubObject.Data;
using Gameplay.Map;
using Gameplay.UI.Game.Canvas;
 using Gameplay.UI.Menu;
 using Gameplay.Units;
using Infrastructure.SceneStates;
using Interface;
 using MaxyGames.uNode.Transition;
 using Plugins.DIContainer;
 using Plugins.HabObject;
 using UnityEngine;

namespace Gameplay.GameSceneScript
{
    public class AvaibleHabToCreate : MonoBehaviour
    {
        [DI] private GameSceneData _sceneData;
        [DI] private GameSceneData _gameSceneData;
        [DI] private FactoryUnit _factoryUnit;
        [DI] private FactoryBuild _factoryBuild;

        private List<Build> _main = new List<Build>();
        private List<Build> _extra = new List<Build>();

        public List<Unit> PlayerUnitOnField = new List<Unit>();
        private List<Build> AllBuildOnMap = new List<Build>();

        public event Action AllPlayerUnitDestroy;
        public event Action<Build> SomeBuildDestroy;

        private void Awake()
        {
            var allbuild = _sceneData.Build;
            _factoryUnit.CreatedT += OnCreateUnit;
            _factoryBuild.CreatedT += OnCreateBuild;
            _main = allbuild.Where(x => x.MainDates.GetOrNull<TypeBuild>().Categor == TypeBuild.Category.Main && CheckAtEmptyNeksus(x)).ToList();
            _extra = allbuild.Where(x => x.MainDates.GetOrNull<TypeBuild>().Categor == TypeBuild.Category.Extra &&  CheckAtEmptyNeksus(x)).ToList();
        }

        private void OnCreateBuild(Build obj)
        {
            AllBuildOnMap.Add(obj);
            obj.Destroyd += OnDestroyBuild;
            
            void OnDestroyBuild(Build objD)
            {
                AllBuildOnMap.Remove(objD);
                obj.Destroyd -= OnDestroyBuild;
                SomeBuildDestroy?.Invoke(objD);
            }
        }

        private void OnCreateUnit(Unit obj)
        {
            if(obj.MainDates.GetOrNull<Team>().Type != Team.Typ.Player)
                return;
            obj.Destroyed += OnDestroyPlayerUnit;
            PlayerUnitOnField.Add(obj);

            void OnDestroyPlayerUnit(HabObject hab)
            {
                obj.Destroyed -= OnDestroyPlayerUnit;
                PlayerUnitOnField.Remove(obj);
                if(PlayerUnitOnField.Count==0)
                    AllPlayerUnitDestroy?.Invoke();
            } 
        }

        private static bool CheckAtEmptyNeksus(Build x) => x.MainDates.GetOrNull<NeksusMark>()==null;

        public List<Unit> GetAvaibleUnit() => _sceneData.PlayerUnits;

        public Build GetNeksus => _gameSceneData.Level.Neksus;

        public List<Build> MainBuilds => _main;
        
        public List<Build> ExtraBuild => _extra;
    }
}