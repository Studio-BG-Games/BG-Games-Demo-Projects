using System;
using Factories;
using Mechanics.GameLevel.Stages.СanistorStageParts.StateMAchine;
using Plugins.DIContainer;
using UnityEngine;

namespace Mechanics.GameLevel.Stages.СanistorStageParts.States
{
    internal class EndingCanistroStageState : CanistroStageState
    {
        [SerializeField] private CanistorStage _stage;

        private Player _player;

        [DI]
        private void Init() => _stage.GetPlayer += OnGetPlayer;

        public override void On()
        {
            FactoryPrompter.ChangeAt(FactoryPrompter.Type.Hello).Say(ConfigLocalization.EndCanistrostage);
            _player.MakeClear();
        }

        public override void Off()
        {
        }

        public override State TransitToOrNull() => null;

        private void OnGetPlayer(Player obj) => _player = obj;
    }
}