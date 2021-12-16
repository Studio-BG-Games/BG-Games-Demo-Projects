using System;
using Gameplay.HubObject.Beh.Damages;
using Plugins.HabObject;
using UnityEngine;

namespace Gameplay.HubObject.Beh.Attack
{
    public abstract class AbsAttack : MonoBehaviour
    {
        public event Action StartAttack;
        public event Action HintMoment;
        public event Action<HabObject> SomeHasAttack;

        public abstract bool InProgress { get; }

        public abstract void Attack();

        protected void CallStartEvet() => StartAttack?.Invoke();
        protected void CallHintEvent() => HintMoment?.Invoke();

        protected void CallSomeHasAttack(HabObject hab) => SomeHasAttack?.Invoke(hab);
    }
}