using System;
using Infrastructure.Configs;
using Plugins.DIContainer;
using UnityEngine;
using Services.Interfaces;
using TMPro;

namespace DefaultNamespace.Services.Console
{
    public class ConsoleTMP : MonoBehaviour, IConsole
    {
        [SerializeField] private TextMeshProUGUI _text;
        
        [DI] private ConfigGame _configGame;

        public void Init()
        {
            DontDestroyOnLoad(gameObject);
            DiBox.MainBox.InjectSingle(this);
            if (!_configGame.IsDebugMode)
                _text.text = "";
        }

        public void Log(string mes)
        {
            if(!_configGame.IsDebugMode)
                return;
            _text.text += mes + "\n";
        }
    }
}