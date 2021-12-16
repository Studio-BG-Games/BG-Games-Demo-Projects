using System;
using Plugins.DIContainer;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Localizators
{
    [RequireComponent(typeof(Button))]
    public class SetOtherLang : MonoBehaviour
    {
        [SerializeField] private Localizator.Lang _targetLang;

        [DI] private Localizator _localizator;

        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(()=>_localizator.SetNewLang(_targetLang));
        }
    }
}