using System;
using Factories;
using Infrastructure.Configs;
using Plugins.DIContainer;

namespace Mechanics.GameLevel.Stages.СanistorStageParts
{
    public class EmptyState : CanistroState
    {
        [DI] private FactoryPrompter _factoryPrompter;
        [DI] private ConfigLocalization _configLocalization;
        
        public EmptyState(Canistro canistro) : base(canistro)
        {
        }
        
        public override void Action(Action callback) => 
            _factoryPrompter.ChangeAt(FactoryPrompter.Type.Fun).Say(_configLocalization.FailChooseCanistro, ()=>callback?.Invoke());
    }
}