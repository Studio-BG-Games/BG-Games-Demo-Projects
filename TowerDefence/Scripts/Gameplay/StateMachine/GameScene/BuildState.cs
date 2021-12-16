using System.Data;
using Factorys;
using Gameplay.GameSceneScript;
using Gameplay.UI.Game.Canvas;
using Plugins.DIContainer;
using UnityEngine;

namespace Gameplay.StateMachine.GameScene
{
    public class BuildState : GameSceneState
    {
        [DI] private FactoryUIForGameScene _factoryUi;
        [DI] private ContainerUIPRefab _contUIPrefab;
        [DI] private PathfinderShell _pathfinderShell;
        [DI] private ControlBuildOnMap _controlBuildOnMap;
        [DI] private Construct _construct;

        public override void Enter()
        {
            var canvas = _factoryUi.GetOrCreate(_contUIPrefab.TemplateChoiseButton, "Choise");
            canvas.gameObject.SetActive(true);
            _factoryUi.GetOrCreate(_contUIPrefab._CanvasWith, "StatCanvas").gameObject.SetActive(true);
            _controlBuildOnMap.On();
        }

        public override void Exit()
        {
            var canvas  = _factoryUi.GetOrCreate(_contUIPrefab.TemplateChoiseButton, "Choise");
            canvas.gameObject.SetActive(false);
            _factoryUi.GetOrCreate(_contUIPrefab._CanvasWith, "StatCanvas").gameObject.SetActive(false);
            _pathfinderShell.Scan();
            _construct.Off();
            _controlBuildOnMap.Off();
        }

        public override void Update()
        {
            
        }
    }
}