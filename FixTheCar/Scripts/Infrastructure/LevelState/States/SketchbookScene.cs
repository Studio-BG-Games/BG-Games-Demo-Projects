using System;
using DefaultNamespace;
using Infrastructure.Configs;
using Plugins.DIContainer;
using Plugins.GameStateMachines.Interfaces;
using Plugins.Interfaces;

namespace Infrastructure.LevelState.States
{
    public class SketchbookScene : IPayLoadedState<ConfigLevel>
    {
        private DiBox _diBox = DiBox.MainBox;

        [DI] private SceneLoader _sceneLoader;
        [DI] private ConfigLevelName _configLevelName;
        [DI] private Curtain _curtain;
        
        public void Enter(ConfigLevel levelConfig)
        {
            _diBox.RegisterSingle(new DataFinishedLevel());
            if(levelConfig)
                _diBox.RegisterSingle(levelConfig);
            _sceneLoader.Load(_configLevelName.SketchBook, ()=>_curtain.Unfade());
        }

        public void Exit()
        {
            _diBox.RemoveSingel<DataFinishedLevel>();
            _diBox.RemoveSingel<ConfigLevel>();
        }
    }
}