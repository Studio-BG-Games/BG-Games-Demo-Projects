using System;
using System.Collections.Generic;
using DefaultNamespace;
using Factories;
using Infrastructure.Configs;
using Infrastructure.LevelState.States;
using Mechanics;
using Mechanics.GameLevel.Stages;
using Mechanics.Interfaces;
using Plugins.DIContainer;
using Plugins.GameStateMachines;
using Plugins.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace Infrastructure.LevelState.SceneScripts.Game
{
    public class InitGame : MonoBehaviour
    {
        public event Action<List<Stage>> Inited;

        [SerializeField] private Button _nextLevelButton;
        [SerializeField] private Transform _parentForStage;

        [DI] private LevelStateMachine _levelStateMachine;
        [DI] private Curtain _curtain;
        [DI] private ConfigLevel _configLevel;
        [DI] private ConfigGame _configGame;
        [DI] private DataFinishedLevel _dataFinishedLevel;
        [DI] private FactoryPrompter _factoryPrompter;

        private DiBox _diBox = DiBox.MainBox;
        private List<Stage> _stages;
        private Player _player;

        private void Awake()
        {
            _nextLevelButton.onClick.AddListener(NextLevel);
            _nextLevelButton.gameObject.SetActive(false);
        }

        private void Start()
        {
            _stages = CreateStages();
            _player = CreatePlayer();
            _stages[0].StartStage(_player, true);
            _stages[0].Completed += OnCompleted;
            Inited?.Invoke(_stages);
            
            _curtain.Unfade();
        }

        private void OnCompleted() => _nextLevelButton.gameObject.SetActive(true);

        private void NextLevel()
        {
            _stages[0].Completed -= OnCompleted;
            _stages.RemoveAt(0);
            if (_stages.Count > 0)
            {
                _stages[0].StartStage(_player, false);
                _stages[0].Completed += OnCompleted;
            }
            else
            {
                _dataFinishedLevel.Add(_configLevel);
                _curtain.Fade(()=>_levelStateMachine.Enter<SketchbookScene, ConfigLevel>(_configLevel)); 
            }
            _nextLevelButton.gameObject.SetActive(false);
        }

        private void OnDestroy() => _nextLevelButton.onClick.RemoveListener(NextLevel);

        private Player CreatePlayer() => _diBox.CreatePrefab(_configGame.PlayerTemplate);

        private List<Stage> CreateStages()
        {
            List<Stage> result = new List<Stage>();
            foreach (var stageData in _configLevel.StagesLevel)
            {
                var stage = _diBox.CreatePrefab(stageData.StageTemplate);
                stage.Init(stageData);
                stage.transform.position = result.Count == 0 ? Vector3.zero : GetPositionStage(result[result.Count - 1], stage);
                stage.transform.SetParent(_parentForStage);
                result.Add(stage);
            }

            return result;
        }

        private Vector3 GetPositionStage(Stage prev, Stage current)
        {
            Vector3 result = prev.transform.position;
            Vector3 offset = new Vector3(prev.SizeElement.Size.x/2+current.SizeElement.Size.x/2, 0, 0);
            return result + offset;
        }
    }
}