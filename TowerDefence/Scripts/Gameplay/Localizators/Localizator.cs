using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace Gameplay.Localizators
{
    [CreateAssetMenu(menuName = "GameSO/Localizator", order = 51)]
    public class Localizator : ScriptableObject
    {
        public event Action NewLangSet;
        
        [SerializeField] private TextAsset[] _texts;
        [SerializeField] private Lang _defalutLand;
        
        private Lang _currentLang;
        private Dictionary<string, FileDict> _idPodLand;

        public string GetText(string nameFile, string id, out bool resultBool)
        {
            if (_idPodLand.TryGetValue(nameFile, out var result))
            {
                if (result.Id_Land.TryGetValue(id, out var r))
                {
                    if (r.Lang_Text.TryGetValue(_currentLang, out var str))
                    {
                        resultBool = true;
                        return str;
                    }
                    else
                    {
                        resultBool = false;
                        Debug.LogError($"у {nameFile}, id {id}, нет языка {_currentLang}");
                        return "False land, check console";
                    }
                }
                else
                {
                    resultBool = false;
                    Debug.LogError($"у {nameFile}, нет id {id}");
                    return "False land, check console";
                }
            }
            else
            {
                resultBool = false;
                Debug.LogError($"нет такого файла - {nameFile}");
                return "False land, check console";
            }
        }

        public void Init(bool withDebug)
        {
            _idPodLand = new Dictionary<string, FileDict>();
            _currentLang = _defalutLand;
            foreach (var text in _texts)
            {
                var file = JsonConvert.DeserializeObject<FileDict>(text.text);
                if(withDebug)
                    Debig(file, text);
                if (file == null || file.Id_Land == null || file.Id_Land.Count == 0)
                    throw new Exception("Ошибка деселизации файла с переводом = "+text.name);
                _idPodLand.Add(text.name, file);
            }
        }

        private static void Debig(FileDict file, TextAsset text)
        {
            if (file == null || file.Id_Land == null || file.Id_Land.Count == 0)
            {
                Debug.LogError($"Неправильно десерилизация у {text.name}");
            }
            else
            {
                Debug.Log($"Правильно десерилизация у {text.name}");
            }

            string toDebug;
            foreach (var idLand in file.Id_Land)
            {
                toDebug = "";
                toDebug += $"id{idLand.Key};";
                foreach (var keyValuePair in idLand.Value.Lang_Text)
                {
                    toDebug += $"Key: {keyValuePair.Key}; Id: {keyValuePair.Value}";
                }

                Debug.Log(toDebug);
            }
        }

        public void SetNewLang(Lang lang)
        {
            _currentLang = lang;
            NewLangSet?.Invoke();
        }

        public enum Lang
        {
            RU, ENG
        }
        
        [System.Serializable]
        public class FileDict
        {
            public Dictionary<string, PodLangDic> Id_Land = new Dictionary<string, PodLangDic>();
        }
        
        public class PodLangDic
        {
            public Dictionary<Lang, string> Lang_Text = new Dictionary<Lang, string>();
        }
    }
}