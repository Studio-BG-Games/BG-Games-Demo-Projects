using Infrastructure.Configs;
using Plugins.DIContainer;
using Plugins.GameStateMachines.Interfaces;
using Plugins.Interfaces;

namespace Infrastructure.LevelState.States
{
    public class MainMenu : IEnterState
    {
        [DI] private SceneLoader _sceneLoader;
        [DI] private Curtain _curtain;
        [DI] private ConfigLevelName _configLevel;
        
        public void Enter()
        {
            _sceneLoader.Load(_configLevel.MainMnu, _curtain.Unfade);
        }

        public void Exit()
        {
            
        }
    }
}