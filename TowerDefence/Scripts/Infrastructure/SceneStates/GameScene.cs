using System;
using System.Collections;
using DefaultNamespace;
using Gameplay;
using Gameplay.GameSceneScript;
using Gameplay.Map;
using Gameplay.Map.ChunkMaster.Scripts.ChunkMaster.ChunkMaster.src.Unity.Framework;
using Gameplay.Map.ConfigData;
using Gameplay.StateMachine.GameScene;
using Infrastructure.ConfigData;
using Interface;
using Plugins.DIContainer;
using Plugins.GameStateMachines.Interfaces;
using Plugins.Interfaces;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Infrastructure.SceneStates
{
    public class GameScene : IPayLoadedState<GameSceneData>
    {
        [DI] private SceneLoader _sceneLoader;
        [DI] private ConfigGame _configGame;
        [DI] private ICoroutineRunner _coroutineRunner;
        [DI] private ICurtainProgress _curtainProgress;
        
        private Optimazer _optimazerGame;
        private DiBox _diBox = DiBox.MainBox;
        private WorldShell _worldShell;
        private GameSceneData _gameSceneData;
        private GameSceneStateMachine _gameSceneStateMachine;

        public void Enter(GameSceneData dataScene)
        {
            _gameSceneData = dataScene;
            _curtainProgress.SetProgress(0);
            CreateEmptyShellWorld(_gameSceneData.DataMap);
            _diBox.RegisterSingle(_gameSceneData);
            _optimazerGame = new Optimazer();
            _diBox.RegisterSingle(_optimazerGame);
            _gameSceneStateMachine = new GameSceneStateMachine();
            _diBox.InjectAndRegisterAsSingle(_gameSceneStateMachine);
            _sceneLoader.Load(_configGame.SceneNames.Game, ()=>FinishLoad(dataScene));
        }

        public void Exit()
        {
            _worldShell.World.Destroy();
            _gameSceneStateMachine.Stop();
            _diBox.RemoveSingel<GameSceneData>();
            _diBox.RemoveSingel<WorldShell>();
            _diBox.RemoveSingel<Optimazer>();
            _diBox.RemoveSingel<GameSceneStateMachine>();
            GC.Collect();
        }

        private void CreateEmptyShellWorld(DataMap mapData)
        {
            var world = new UnityWorld(new GameObject("Game World On Game Scene").transform);
            _worldShell = new WorldShell(world, mapData);
            _diBox.RegisterSingle(_worldShell);
            Object.DontDestroyOnLoad(world.gameObject);
        }

        private void FinishLoad(GameSceneData dataScene)
        {
            dataScene.GeneratorMap.Progress += MakeProgress;
            dataScene.GeneratorMap.FinishedGeneration += OnFinishedGenerate;
            dataScene.GeneratorMap.SectorCreate += _optimazerGame.OptimazeSector;
            dataScene.GeneratorMap.Generate(dataScene.DataMap, _worldShell.World);
            
        }

       private void OnFinishedGenerate()
        {
            _curtainProgress.Unfade();
            _optimazerGame.OptimazeWorld(_worldShell.World);
            _gameSceneData.GeneratorMap.Progress -= MakeProgress;
            _gameSceneData.GeneratorMap.FinishedGeneration -= OnFinishedGenerate;
            _gameSceneData.GeneratorMap.SectorCreate -= _optimazerGame.OptimazeSector;
            _gameSceneStateMachine.Enter<BuildNeksusState>();
            var pathinder = DiBox.MainBox.ResolveSingle<PathfinderShell>();
            pathinder.UpdateTag(_worldShell);
            pathinder.Scan();
        }

        private void MakeProgress(float normalProgress) => _curtainProgress.SetProgress(normalProgress);
    }
}