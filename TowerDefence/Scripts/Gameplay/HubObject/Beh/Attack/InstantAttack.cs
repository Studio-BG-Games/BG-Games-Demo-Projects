using System.Collections;
using Gameplay.HubObject.Beh.Attributes;
using Plugins.DIContainer;
using Plugins.Interfaces;
using UnityEngine;

namespace Gameplay.HubObject.Beh.Attack
{
    [BehaviourButton("Combat/Attack/InstantAttack")]
    public class InstantAttack : AbsAttack
    {
        [SerializeField] private GiveTarget _giveTarget;
        [SerializeField] private AttackContainerBeh _attackContainerBeh;

        [DI] private ICoroutineRunner _coroutineRunner;
        
        public override bool InProgress => false;
        public override void Attack() => _coroutineRunner.StartCoroutine(Damage());

        private IEnumerator Damage()
        {
            yield return null;
            var targets = _giveTarget.All();
            bool callHint = false;
            CallStartEvet();
            for (var i = 0; i < targets.Count; i++)
            {
                if (targets[i])
                {
                    var h = targets[i].ComponentShell.Get<Health>();
                    if (h)
                    {
                        h.Damage(_attackContainerBeh.GetDamage());
                        CallSomeHasAttack(targets[i]);
                        if (!callHint)
                        {
                            CallHintEvent();
                            callHint = true;
                        }
                    }
                }
            }
        }
    }
}