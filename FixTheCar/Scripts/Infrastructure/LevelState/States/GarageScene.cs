using Factories;
using Infrastructure.Configs;
using Infrastructure.LevelState.SceneScripts.Garages;
using Plugins.DIContainer;
using Plugins.GameStateMachines.Interfaces;
using Plugins.Interfaces;

namespace Infrastructure.LevelState.States
{
    public class GarageScene : IEnterState
    {
        [DI] private SceneLoader _sceneLoader;
        [DI] private ConfigLevelName _configLevel;
        [DI] private Curtain _curtain;

        private DiBox _diBox = DiBox.MainBox;
        
        public void Enter()
        {
            CreateDI(new SelectorCar());
            _sceneLoader.Load(_configLevel.Garage);
        }

        public void Exit()
        {
            _diBox.RemoveSingel<SelectorCar>();
            _diBox.RemoveSingel<FactoryGarage>();
        }

        public void CreateDI(SelectorCar selectorCar)
        {
            _diBox.RegisterSingle(selectorCar);
            _diBox.RegisterSingle(new FactoryGarage());
            
            _diBox.InjectSingle(selectorCar);
        }
    }
}