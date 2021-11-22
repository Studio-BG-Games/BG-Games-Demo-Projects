using System;
using System.Collections.Generic;
using Factories;
using Mechanics;
using Mechanics.Garage;
using Mechanics.Prompters;
using Services.IInputs;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Infrastructure.Configs
{
    [CreateAssetMenu(menuName = "Config/Game", order = 51)]
    public class ConfigGame : ScriptableObject
    {
        public bool IsDebugMode;
        public bool IsPointAndClick;
        public Player PlayerTemplate => IsPointAndClick ? _playerOnPointAndClick : _playerOnButton;
        public Garage TemplateGarage;
        public int startSizePoolSource = 15;
        public AudioSource TemplateSource;
        [Tooltip("По горизонтале колличество символов в строке, по вертикале скорость набора")]
        public AnimationCurve SpeedCurveTextPrompter;
        public List<PrompterType> TemplatePrompter;
        public List<ConfigLevel> ConfigLevels;
        public CanvasUiInput CanvasUI;

        [SerializeField] private Player _playerOnButton;
        [SerializeField] private Player _playerOnPointAndClick;
        
        [Serializable]
        public class PrompterType
        {
            public Prompter Template => _template;
            public FactoryPrompter.Type Type => _type;

            [SerializeField] private Prompter _template;
            [SerializeField] private FactoryPrompter.Type _type;
        }
    }
}