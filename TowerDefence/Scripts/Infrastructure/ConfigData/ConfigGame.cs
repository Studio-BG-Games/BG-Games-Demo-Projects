using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.Ad;
using DefaultNamespace.Infrastructure.Data;
using Extension;
using Gameplay.Builds;
using Gameplay.HubObject.Data;
using Gameplay.Units;
using Plugins.HabObject;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Infrastructure.ConfigData
{
    [CreateAssetMenu(menuName = "GameSO/Config/ConfigGame", order = 51)]
    public class  ConfigGame : ScriptableObject
    {
        [UnityEngine.Header("Все юниты и здания")]
        public SceneNames SceneNames;

        [UnityEngine.Header("Все юниты и здания")]
        public ConfigContainerBuild ConfigContainerBuild;
        public ConfigContainerUnit ConfigContainerUnit;

        [Header("Максимальное колличесвто юнитов и здании в пуле")] 
        [Min(1)] public int MaxCountBuild;
        [Min(1)] public int MaxCountUnit;
        
        [UnityEngine.Header("Набор уровней")]
        public List<LevelsSet> LevelsSets;

        [UnityEngine.Header("Профайлеры")]
        public List<PlayerUnitProfile> PlayerUnitProfiles;
        public List<PlayerBuildProfile> PlayerBuildProfiles;
        public List<EnemyUnitProfile> EnemyUnitProfiles;
        public List<LevelSetProfile> LevelSetProfiles; 

        public void InitConfig(SaveDataProvider provider)
        {
            PlayerUnitProfiles.ForEach(x=>x.UpdateData(provider));
            PlayerBuildProfiles.ForEach(x=>x.UpdateData(provider));
            EnemyUnitProfiles.ForEach(x=>x.UpdateData(provider));
            LevelSetProfiles.ForEach(x=>x.UpdateData(provider));
        }

        private void OnValidate()
        {
            ConfigContainerBuild.OnValidate();
            ConfigContainerUnit.OnValidate();

            PlayerUnitProfiles.ForEach(x=>x.OnValidate());
            PlayerBuildProfiles.ForEach(x=>x.OnValidate());
            EnemyUnitProfiles.ForEach(x=>x.OnValidate());
            LevelSetProfiles.ForEach(x=>x.OnValidate());
                
            LevelsSets.ForEach(x=>x?.OnValidate());
            var dict = new Dictionary<string, object>();
            foreach (var levelsSet in LevelsSets)
            {
                if(!levelsSet)
                    break;
                while (dict.ContainsKey(levelsSet.ID))
                {
                    levelsSet.Regenerate();
                }
                dict.Add(levelsSet.ID, levelsSet);
            }

            LevelsSets = LevelsSets.OrderBy(x => x?.Priority).ToList();
        }
    }

    [System.Serializable]
    public class SceneNames
    {
        public string Menu => _menu;
        public string Game => _game;
        [SerializeField] private string _menu;
        [SerializeField] private string _game;
    }
    
    [System.Serializable]
    public abstract class ConfigContainerHub<T> where T : HabObject
    {
        [SerializeField] protected List<T> List;
        
        [NonSerialized] private Dictionary<string, T> _idAndObject;

        public Dictionary<string, T> IdAndObject
        {
            get
            {
                if (_idAndObject!=null)
                    return _idAndObject;
                _idAndObject = new Dictionary<string, T>();
                List.ForEach(x =>
                {
                    _idAndObject.Add(x.MainDates.GetOrNull<IdContainer>().ID, x);
                });
                return _idAndObject;
            }
        }

        public List<T> GetAll(Func<T, bool> predict)
        {
            List<T> result = new List<T>();
            List.ForEach(x =>
            {
                if(predict.Invoke(x))
                    result.Add(x);
            });
            return result;
        }

        public T GetTByIdOrNull(string id)
        {
            IdAndObject.TryGetValue(id, out var result);
            return result;
        }
        
        public static string GetIdByT(T template) => template.MainDates.GetOrNull<IdContainer>().ID;

        public void OnValidate()
        {
            List.RemoveAll(x => x == null);
            AddOnValidate();
            Dictionary<string, T> idAndObject = new Dictionary<string, T>();
            foreach (var habObject in List)
            {
                var idC = habObject.MainDates.GetOrNull<IdContainer>();
                while (idAndObject.ContainsKey(idC.ID))
                {
                    HelperID.GenerateID(idC);
                    #if UNITY_EDITOR
                    EditorUtility.SetDirty(idC);
                    #endif
                }
                idAndObject.Add(idC.ID, habObject);
            }
        }

        protected virtual void AddOnValidate() { }
    }

    [System.Serializable]
    public class ConfigContainerUnit : ConfigContainerHub<Unit>
    {
     
    }
    
    [System.Serializable]
    public class ConfigContainerBuild : ConfigContainerHub<Build>
    {
        
    }
}