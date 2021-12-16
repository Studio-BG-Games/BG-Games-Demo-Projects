using System;
using System.Collections.Generic;
using Gameplay.Map.ConfigData;
using Gameplay.Waves;
using Infrastructure.SceneStates;
using Interface;
using Plugins.GameStateMachines;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using Random = UnityEngine.Random;

namespace DefaultNamespace.Infrastructure.Data
{
    [CreateAssetMenu(menuName = "GameSO/Level set", order = 49)]
    public class LevelsSet : ScriptableObject, IIDContainer
    {
        public int CountLevel => _levels.Count;
        
        public LevelAndSet GetLevelSet(int index) => _levels[index];
        [SerializeField] private List<LevelAndSet> _levels;

        public DataMap DataMap => _dataMapSo.Value;
        [SerializeField] private DataMapSo _dataMapSo;

        public string ID => _id;
        [SerializeField] [HideInInspector]private string _id;

        public int Priority => _priority;
        [SerializeField] private int _priority;
        
        private static string GenerateID(ScriptableObject scriptableObject) => scriptableObject.name + "+" + Guid.NewGuid().ToString();

       
        public void Regenerate()
        {
            _id = GenerateID(this);
            #if UNITY_EDITOR
            EditorUtility.SetDirty(this);
            #endif
        }

        public void OnValidate()
        {
            if (string.IsNullOrEmpty(_id))
                _id = GenerateID(this);
            if(_levels.Count==0)
                Debug.LogError($"{name} не имеет уровней");
            for (var i = 0; i < _levels.Count; i++) _levels[i].OnValidate(this, i);
        }

        [System.Serializable]
        public class LevelAndSet
        {
            public Level Level;
            public bool WithWater;
            public int SeedLansScape;
            public int SeedPropBuild;
            public Vector2 PostionNeksus;
            [Min(0)]public int AwardForLevel;

            public void OnValidate(LevelsSet levelsSet, int id)
            {
                if(!Level)
                    Debug.LogError($"{levelsSet.name} на {id} позиции не имеет уровня, фикс ит");
                if (SeedLansScape == 0)
                {
                    SeedLansScape = Random.Range(-50000,50000);
                }
                if (SeedPropBuild == 0)
                {
                    SeedPropBuild = Random.Range(-50000,50000);
                }
            }
        }
    }
}