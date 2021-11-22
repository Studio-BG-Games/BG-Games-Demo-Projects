using System;
using Factories;
using Infrastructure.Configs;
using Plugins.DIContainer;
using UnityEngine;

namespace Mechanics.GameLevel.Stages.BossStagePart.StateMachine.State
{
    public class WinFightWithBoss : StageBossState
    {
        [DI] private FactoryPrompter _factoryPrompter;
        [DI] private ConfigLocalization _configLocalization;

        public override void On()
        {
            _factoryPrompter.ChangeAt(FactoryPrompter.Type.Fun).Unhide(()=>_factoryPrompter.Current.Say(_configLocalization.WinBossStage));
        }

        public override void Off()
        {
        }

        public override Stages.State TransitToOrNull() => null;
    }
}