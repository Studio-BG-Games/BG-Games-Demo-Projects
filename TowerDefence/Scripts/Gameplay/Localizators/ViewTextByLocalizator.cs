
using System;
using Plugins.DIContainer;
using UnityEngine;

namespace Gameplay.Localizators
{
    public abstract class ViewTextByLocalizator : MonoBehaviour
    {
        [SerializeField] private string _fileName;
        [SerializeField] private string _id;

        public Localizator Localizator => _localizator ??=DiBox.MainBox.ResolveSingle<Localizator>();
        private Localizator _localizator;

        private void Awake()
        {
            CustomAwake();
            Localizator.NewLangSet += OnNewLang;
            OnNewLang();
        }

        private void OnNewLang()
        {
            var str = Localizator.GetText(_fileName, _id, out var resultBool);
            if (!resultBool)
            {
                Debug.LogError($"Установка текста из серилизатора неправильная, проверьте этот объект {gameObject.name}, кликни на мне два раза", gameObject);
            }
            SetText(str);
        }

        protected abstract void SetText(string text);

        protected abstract void CustomAwake();
    }
}