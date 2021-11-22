using Factories;
using Infrastructure.Configs;
using Plugins.DIContainer;
using Services.Interfaces;

namespace Mechanics.GameLevel.Stages.СanistorStageParts.StateMAchine
{
    public abstract class CanistroStageState : State
    {
        [DI] protected ConfigLocalization ConfigLocalization;
        [DI] protected FactoryPrompter FactoryPrompter;
        [DI] protected IInput InputPlayer;
    }
}