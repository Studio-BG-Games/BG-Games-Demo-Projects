using System;
using System.ComponentModel;
using UnityEngine;

namespace Gameplay
{
    [BehaviourButton("Setter Target AI/As my position")]
    public class SetTargetAIAsZero : AbsSetterTargetAI
    {
        [SerializeField]private string nothing;

        public override Vector3 GetDestination() => Unit.transform.position;
    }
}