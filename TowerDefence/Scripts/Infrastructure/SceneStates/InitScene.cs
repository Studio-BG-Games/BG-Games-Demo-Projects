using System;
using DefaultNamespace.Infrastructure.Data;
using Gameplay.Map.ConfigData;
using Interface;
using Plugins.DIContainer;
using Plugins.GameStateMachines;
using Plugins.GameStateMachines.Interfaces;
using Plugins.Interfaces;

namespace Infrastructure.SceneStates
{
    public class InitScene : IEnterState
    {
        [DI] private AppStateMachine _appStateMachine;
        [DI] private ICurtain _curtain;

        private DiBox _diBox = DiBox.MainBox;
        
        public void Enter()
        {
            _diBox.InjectAndRegisterAsSingle(new SceneLoader());
            _curtain.Fade(
                () =>
                {
                    _curtain.Fade(()=>_appStateMachine.Enter<MainMenu>());
                }, 0);
            //_curtainProgress.Fade(()=>_appStateMachine
                //.Enter<GameScene, GameSceneData>(new GameSceneData(_dataMap.Value, data.GeneratorMap, data.Buildses, data.Units, data.Level)));
        }

        public void Exit()
        {
            
        }
    }
}