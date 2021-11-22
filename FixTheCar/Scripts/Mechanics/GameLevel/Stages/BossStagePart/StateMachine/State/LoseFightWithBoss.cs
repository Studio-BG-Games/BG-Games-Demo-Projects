using Factories;
using Infrastructure.Configs;
using Plugins.DIContainer;
using Plugins.Interfaces;
using Services.Interfaces;
using TMPro;
using UnityEngine;

namespace Mechanics.GameLevel.Stages.BossStagePart.StateMachine.State
{
    public class LoseFightWithBoss : StageBossState
    {
        [SerializeField] private BossStage _bossStage;
        [SerializeField] private FightWithBoss _fightWithBoss;
        
        [DI] private FactoryPrompter _factoryPrompter;
        [DI] private ConfigLocalization _configLocalization;
        [DI] private IInput _input;
        [DI] private Curtain _curtain;

        private bool _isTransit = false;
        
        public override void On()
        {
            _factoryPrompter.ChangeAt(FactoryPrompter.Type.WTF).Unhide(
                ()=>_factoryPrompter.Current.Say(_configLocalization.LoseBossStage,
                    ()=>_input.AnyInput+=OnAnyInput));
        }

        private void OnAnyInput()
        {
            _input.AnyInput -= OnAnyInput;
            _factoryPrompter.Current.Hide(
                ()=>_curtain.Transit(
                    ()=>_bossStage.Restart(
                        ()=>_isTransit=true)
                    )
                );
        }

        public override void Off() => _isTransit = false;

        public override Stages.State TransitToOrNull() => _isTransit ? _fightWithBoss : null;
    }
}