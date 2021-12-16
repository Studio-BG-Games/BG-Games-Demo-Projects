using Gameplay.Units;
using UnityEngine;

namespace Gameplay
{
    [BehaviourButton("Setter Target AI/From Container")]
    public class SetTargetAIFromContaner : AbsSetterTargetAI
    {
        private TargetContainer _targetContainer;

        private void Awake() => _targetContainer = Unit.MainDates.GetOrNull<TargetContainer>();

        public override Vector3 GetDestination() => _targetContainer.TargetMove;
    }
}