using System;
using System.Linq;
using Factorys;
using Gameplay.Builds;
using Gameplay.Builds.Data.Marks;
using Gameplay.GameSceneScript;
using Gameplay.UI;
using Infrastructure.SceneStates;
using Plugins.DIContainer;

namespace Gameplay.StateMachine.GameScene
{
    public class BuildNeksusState : GameSceneState
    {
        private bool _isEntered;

        [DI] private GameSceneData _gameSceneData;
        [DI] private Construct _construct;
        [DI] private ControlBuildOnMap _controlBuildOnMap;
        [DI] private FactoryBuild _factory;
        [DI] private UIMediatorGameScene _mediatorGameScene;
        
        public override void Enter()
        {
            if(_isEntered) throw  new Exception("You arledy entered to this state");
            _controlBuildOnMap.Off();
            _factory.CreatedT += OnBuildCreated;
            _construct.On<ConstructNeksusState>();
        }

        private void OnBuildCreated(Build obj)
        {
            _mediatorGameScene.ChangeState<BuildState>();
        }

        public override void Exit()
        {
            _factory.CreatedT -= OnBuildCreated;
            _construct.Off();
        }

        public override void Update()
        {
            
        }
    }
}