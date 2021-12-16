using Factorys;
using Gameplay.GameSceneScript;
using Plugins.DIContainer;
using UnityEngine;

namespace Gameplay.StateMachine.GameScene
{
    public class LoseState : GameSceneState
    {
        [DI] private FactoryUIForGameScene _factoryUiForGameScene;
        [DI] private ContainerUIPRefab _containerUipRefab;
        
        public override void Enter()
        {
            _factoryUiForGameScene.GetOrCreate(_containerUipRefab.LoseCanvas, "sojpsndv").gameObject.SetActive(true);
        }

        public override void Exit()
        {
            _factoryUiForGameScene.GetOrCreate(_containerUipRefab.LoseCanvas, "sojpsndv").gameObject.SetActive(false);
        }

        public override void Update()
        {
        }
    }
}