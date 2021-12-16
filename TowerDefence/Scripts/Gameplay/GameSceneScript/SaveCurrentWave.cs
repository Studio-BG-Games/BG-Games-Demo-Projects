using System;
using DefaultNamespace.Infrastructure.Data;
using Gameplay.StateMachine.GameScene;
using Infrastructure.SceneStates;
using Plugins.DIContainer;
using UnityEditor;
using UnityEngine;

namespace Gameplay.GameSceneScript
{
    public class SaveCurrentWave : MonoBehaviour
    {
        [DI] private GameSceneStateMachine _gameSceneState;
        [DI] private InterpreterWaveLevel _interpreterWave;
        [DI] private GameSceneData _sceneData;
        [DI] private IGold _gold;

        public int SaveLastNubmerWave => PlayerPrefs.HasKey(IDCurrentSesion) ? PlayerPrefs.GetInt(IDCurrentSesion) : 0;
        
        public int GetGoldByAllAward
        {
            get
            {
                int maxIndex = SaveLastNubmerWave;
                int award = 0;
                for (var i = 0; i < maxIndex; i++)
                {
                    award += _sceneData.Level.Waves[i].Award;
                }

                return award;
            }
        }
#if UNITY_EDITOR
        [MenuItem("SaveLevelWaveProgress/Delete all")]
        private void ClearAllWave()
        {
            PlayerPrefs.DeleteAll();
        }
#endif
        private string IDCurrentSesion => $"Level-{_sceneData.LevelSetProfile.CurrentData.CurrentIndexLevel}-in-{_sceneData.LevelSetProfile.Target.ID}";

        private void Awake()
        {
            _gold.Add(GetGoldByAllAward);
            _gameSceneState.EnteredTo += OnEnterToBuild;
            _gameSceneState.EnteredTo += OnEnterToWinLose;
        }

        private void OnEnterToBuild(Type obj)
        {
            if (obj == typeof(BuildState))
            {
                PlayerPrefs.SetInt(IDCurrentSesion, _interpreterWave.IndexCurrentWave);
            }
        }

        private void OnEnterToWinLose(Type obj)
        {
            if (obj == typeof(WinState) || obj == typeof(LoseState))
            {
                PlayerPrefs.DeleteKey(IDCurrentSesion);
            }
        }
    } 
}