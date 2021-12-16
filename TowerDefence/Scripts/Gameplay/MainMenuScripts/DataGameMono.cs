using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.Infrastructure.Data;
using Infrastructure.ConfigData;
using Plugins.DIContainer;
using UnityEngine;

namespace Gameplay.UI.Menu
{
    public class DataGameMono : MonoBehaviour
    {
        [DI(SaveDataProvider.OnlineID)] private SaveDataProvider _saveDataProvider;
        [DI] private ConfigGame _configGame;
        
        private DataGame _data;
        
        public int CurrentGold => _data.MainMoney;
        public bool BuildPullIsFull => _data.BuildPull.Length >= _configGame.MaxCountBuild;
        public bool UnitPullIsFull => _data.UnitPull.Length >= _configGame.MaxCountUnit;
        public int MaxCountUnit => _configGame.MaxCountUnit;
        public int MaxCountBuild => _configGame.MaxCountBuild;

        public event Action BuildPoolHasChahge;
        public event Action UnitPoolHasChahge;

        private void Awake()
        {
            _data = _saveDataProvider.GetOrDefault<DataGame>();
            SaveCurrentData();
        }

        public void SaveCurrentData() => _saveDataProvider.Save(_data);

        public void SetGoldTo(int i)
        {
            if (i < 0) i = 0;
            _data.MainMoney = i;
            SaveCurrentData();
        }

        public string[] GetBuildPull() => _data.BuildPull;

        public string[] GetUnitPull() => _data.UnitPull;

        public void AddToPullBuild(string id)
        {
            var lsitId =GetBuildPull().ToList();
            if(lsitId.Contains(id))
                return;
            lsitId.Add(id);
            _data.BuildPull = lsitId.ToArray();
            SaveCurrentData();
            BuildPoolHasChahge?.Invoke();
        }

        public void RemoveFromPullBuild(string id)
        {
            var list = GetBuildPull().ToList();
            list.RemoveAll(x => x == id);
            _data.BuildPull = list.ToArray();
            SaveCurrentData();
            BuildPoolHasChahge?.Invoke();
        }

        public void AddToPullUnitNewElelment(string id)
        {
            if(_data.UnitPull.ToList().Contains(id)) return;
            var list = _data.UnitPull.ToList();
            list.Add(id);
            _data.UnitPull = list.ToArray();
            SaveCurrentData();
            UnitPoolHasChahge?.Invoke();
        }

        public void RemovePlayerPullElement(string id)
        {
            var playerPools = GetUnitPull().ToList();
            playerPools.RemoveAll(x => x == id);
            _data.UnitPull = playerPools.ToArray();
            SaveCurrentData();
            UnitPoolHasChahge?.Invoke();
        }
    }
}