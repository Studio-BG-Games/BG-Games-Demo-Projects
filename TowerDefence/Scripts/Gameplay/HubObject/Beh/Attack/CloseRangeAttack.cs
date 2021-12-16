using System;
using System.Collections.Generic;
using Gameplay.HubObject.Beh.Attributes;
using Gameplay.Units;
using Plugins.HabObject;
using UnityEngine;

namespace Gameplay.HubObject.Beh.Attack
{
    [BehaviourButton("Combat/Attack/Close range attack")]
    public class CloseRangeAttack : AbsAttack
    {
        public override bool InProgress => _animatorApplay.IsCurrentAnimation(_nameAnimationAttack, 0);

        [SerializeField] private AnimatorApplayCallback _animatorApplay;
        [SerializeField] private string _nameAnimationAttack;
        [SerializeField] private string _nameCallbackHint;
        [SerializeField] private GiveTarget _giveTarget;
        [SerializeField] private AttackContainerBeh _attackContainer;
        private List<HabObject> _targets;


        public override void Attack()
        {
            if(InProgress)
                return;
            CallStartEvet();
            _animatorApplay.StartAnimation(_nameAnimationAttack);
            _animatorApplay.CallbackAnimation += OnHint;
            _targets = _giveTarget.All();
            foreach (var habObject in _targets)
            {
                habObject.ComponentShell.Get<Health>().Dead += OnDeadHab;
            }
        }

        private void OnDeadHab(HabObject obj)
        {
            _targets?.Remove(obj);
            obj.ComponentShell.Get<Health>().Dead -= OnDeadHab;
            if(_targets!=null)
                if (_targets.Count == 0)
                    _targets = null;
        }

        private void OnHint(string obj)
        {
            _animatorApplay.CallbackAnimation -= OnHint;
            if(obj!=_nameCallbackHint)
                return;
            CallHintEvent();
            if(_targets!=null )
            {
                if(_targets.Count>0)
                {
                    for (var i = 0; i < _targets.Count; i++)
                    {
                        if (i >= _targets.Count)
                            break;
                        if (!_targets[i])
                            continue;
                        var h = _targets[i].ComponentShell.Get<Health>();
                        if (h)
                        {
                            if(_targets[i])
                                CallSomeHasAttack(_targets[i]);
                            h.Damage(_attackContainer.GetDamage());
                        }
                    }
                }
            }
            _targets = null;
        }
        
    }
}