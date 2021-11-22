using System.Collections.Generic;
using Infrastructure.Configs;
using Newtonsoft.Json;
using UnityEngine;

namespace DefaultNamespace
{
    public class DataFinishedLevel
    {
        private const string IdSavedFinishedLevel = "FinishedLevelID";

        public List<string> GetAllFinishedLevel()
        {
            var stringResult = PlayerPrefs.GetString(IdSavedFinishedLevel, JsonConvert.SerializeObject(new List<string>()));
            return JsonConvert.DeserializeObject<List<string>>(stringResult);
        }
        
        public void Clear()
        {
            List<string> emptyList = new List<string>();
            PlayerPrefs.SetString(IdSavedFinishedLevel, JsonConvert.SerializeObject(emptyList));
        }

        public void Add(ConfigLevel level)
        {
            var listName = GetAllFinishedLevel();
            if(!listName.Contains(level.name))
                listName.Add(level.name);
            PlayerPrefs.SetString(IdSavedFinishedLevel, JsonConvert.SerializeObject(listName));            
        }
    }
}