using System;
using Factories;
using Infrastructure.Configs;
using Plugins.DIContainer;

namespace Mechanics.GameLevel.Stages.СanistorStageParts
{
    public class OilState : CanistroState
    {
        private readonly Canistro _canistro;
        private readonly Player _player;
        [DI] private FactoryPrompter _factoryPrompter;
        [DI] private ConfigLocalization _configLocalization;

        public OilState(Canistro canistro, Player player) : base(canistro)
        {
            _canistro = canistro;
            _player = player;
        }

        public override void Action(Action callback)
        {
            _factoryPrompter.ChangeAt(FactoryPrompter.Type.Fun).Say(_configLocalization.FailChooseCanistro);
            _canistro.Show(2, ()=>
            {
                _canistro.Hide(2, () => callback?.Invoke());
                _canistro.ChangeValueFuel(0,2f, ()=>_canistro.ChangeState<EmptyState>());
            });
            _player.MakeDirty();
        }
    }
}