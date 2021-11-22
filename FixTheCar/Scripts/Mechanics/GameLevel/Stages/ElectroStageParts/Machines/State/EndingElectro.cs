using Factories;
using Infrastructure.Configs;
using Mechanics.GameLevel.Stages.BossStagePart.Interfaces;
using Plugins.DIContainer;
using UnityEngine;

namespace Mechanics.GameLevel.Stages.ElectroStageParts.Machines.State
{
    public class EndingElectro : ElectorState, IInitPlayer
    {
        [DI] private FactoryPrompter _factoryPrompter;
        [DI] private ConfigLocalization _configLocalization;
        private Player _player;

        public override void On()
        {
            _factoryPrompter.ChangeAt(FactoryPrompter.Type.Idea);
            _factoryPrompter.Current.Say(_configLocalization.FinishElectroStage);
            _player.GetComponent<SpriteRenderer>().flipX = false;
        }

        public override void Off()
        {
        }

        public override Stages.State TransitToOrNull() => null;
        public void Init(Player player) => _player = player;
    }
}