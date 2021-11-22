using System;
using System.Collections;
using Factories;
using Infrastructure.Configs;
using Mechanics.GameLevel.Stages.BossStagePart.Interfaces;
using Mechanics.Prompters.Interfaces;
using Plugins.DIContainer;
using Services.Interfaces;
using UnityEngine;

namespace Mechanics.GameLevel.Stages.ElectroStageParts.Machines.State
{
    public class GreetingsElectro : ElectorState, IInitPlayer
    {
        [SerializeField] private SetWiresState _setWiresState;
        [SerializeField] private InteractWithWirePart _interactWithWire;
        [SerializeField] private AbsDelayPrometed _delay;
        
        [DI] private ConfigLocalization _configLocalization;
        [DI] private FactoryPrompter _factoryPrompter;
        [DI] private IInput _input;

        private bool _transitTo = false;
        private Player _player;

        public override void On()
        {
            _player.GetComponent<SpriteRenderer>().flipX = true;
            StartCoroutine(StopInterectOnNewFrame());
            _factoryPrompter.ChangeAt(FactoryPrompter.Type.Hello);
            _factoryPrompter.Current.Unhide(() =>
            {
                _factoryPrompter.Current.Say(_configLocalization.HelloElectroStage, 
                    ()=>_delay.Activated(NextAction));
            });
        }

        public override void Off()
        {
            _transitTo = false;
            _input.AnyInput -= NextAction;
        }

        public override Stages.State TransitToOrNull() => _transitTo ? _setWiresState : null; 

        private void NextAction() => _transitTo = true;

        private IEnumerator StopInterectOnNewFrame()
        {
            yield return null;
            _interactWithWire.OffInteract();
        }

        public void Init(Player player) => _player = player;
    }
}