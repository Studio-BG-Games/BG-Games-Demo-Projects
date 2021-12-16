using UnityEngine;

namespace Gameplay
{
    [BehaviourButton("Setter Target AI/Some vector")]
    public class SetTargetAISomeVector : AbsSetterTargetAI
    {
        private Vector3 _target;
        public void Init(Vector3 target) => _target= target;
        
        public override Vector3 GetDestination() => _target;
    }
}