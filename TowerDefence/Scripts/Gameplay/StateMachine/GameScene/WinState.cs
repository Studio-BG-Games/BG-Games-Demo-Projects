using Factorys;
using Gameplay.GameSceneScript;
using Plugins.DIContainer;
using UnityEngine;

namespace Gameplay.StateMachine.GameScene
{
    public class WinState : GameSceneState
    {
        [DI] private FactoryUIForGameScene _factoryUiForGameScene;
        [DI] private ContainerUIPRefab _containerUipRefab; 
        
        public override void Enter()
        {
            _factoryUiForGameScene.GetOrCreate(_containerUipRefab.WinCanvas, "dfhcghfdgb").gameObject.SetActive(true);
        }

        public override void Exit()
        {
            _factoryUiForGameScene.GetOrCreate(_containerUipRefab.WinCanvas, "dfhcghfdgb").gameObject.SetActive(false);
        }

        public override void Update()
        {
        }
    }
}