using System;
using Factories;
using Infrastructure.Configs;
using Plugins.DIContainer;

namespace Mechanics.GameLevel.Stages.СanistorStageParts
{
    public class FuelState : CanistroState
    {
        [DI] private FactoryPrompter _factoryPrompter;
        [DI] private ConfigLocalization _configLocalization;

        private BigCanistro _bigCanistro;
        private readonly Canistro _canistro;

        public FuelState(BigCanistro bigCanistro, Canistro canistro) : base(canistro)
        {
            _bigCanistro = bigCanistro;
            _canistro = canistro;
        }

        public override void Action(Action callback)
        {
            _canistro.Show(2, () =>
            {
                _bigCanistro.IncreseAndCallIfNotFinished(2.1f, ()=>callback?.Invoke());
                _canistro.ChangeValueFuel(0, 2, ()=>
                {
                    _canistro.ChangeState<EmptyState>();
                    _canistro.Hide(0.0001f);
                });
            });
            _factoryPrompter.ChangeAt(FactoryPrompter.Type.Idea).Say(_configLocalization.СorrectlyChooseCanistro);
        }
    }
}