using System;
using System.Collections.Generic;
using Infrastructure.Configs;
using Mechanics.Prompters;
using Plugins.DIContainer;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Factories
{
    public class FactoryPrompter : MonoBehaviour
    {
        public Prompter Current => _dictPrompter[_current];
        
        [SerializeField] private HorizontalLayoutGroup _placePromter;
        
        [DI] private ConfigGame _configGame;
        
        private Dictionary<Type, Prompter> _dictPrompter;
        private Type _current;

        [DI] private void Init()
        {
            _dictPrompter = new Dictionary<Type, Prompter>();
            foreach (var listPair in _configGame.TemplatePrompter)
            {
                var instance = DiBox.MainBox.CreatePrefab(listPair.Template);
                _dictPrompter.Add(listPair.Type, instance);
                instance.transform.SetParent(_placePromter.transform);
                instance.gameObject.SetActive(false);
            }
        }

        public void ChangeAndSayAnimated(Type type, string mes, Action callback = null)
        {
            _dictPrompter[_current].Hide(() =>
            {
                ChangeAt(type).Unhide(() => Current.Say(mes, callback));
            });
        }
        
        public void ChangeAndSayNoneAnimated(Type type, string mes, Action callback = null) => ChangeAt(type).Say(mes,callback);

        public Prompter ChangeAt(Type newType)
        {
            if(_dictPrompter.TryGetValue(_current, out var result))
                result.gameObject.SetActive(false);
            var newCurrentPromter = _dictPrompter[newType];
            newCurrentPromter.gameObject.SetActive(true);
            if(result)
                newCurrentPromter.Init(result);
            _current = newType;
            return _dictPrompter[newType];
        }
        
        public enum Type
        {
            Hello, DontKnow, Idea, Fun, WTF
        }
    }
}