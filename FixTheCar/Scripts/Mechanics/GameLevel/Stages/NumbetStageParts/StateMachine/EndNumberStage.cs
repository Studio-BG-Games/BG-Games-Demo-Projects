using Factories;
using Infrastructure.Configs;
using Plugins.DIContainer;

namespace Mechanics.GameLevel.Stages.NumbetStageParts.StateMachine
{
    public class EndNumberStage : NumberStageState
    {
        [DI] private FactoryPrompter _factoryPrompter;
        [DI] private ConfigLocalization _configLocalization;
        
        public override void On() => _factoryPrompter.ChangeAndSayNoneAnimated(FactoryPrompter.Type.Hello, _configLocalization.EndNumberStage);

        public override void Off()
        {
        }

        public override State TransitToOrNull() => null;
    }
}