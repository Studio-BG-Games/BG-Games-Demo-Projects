using Gameplay.Units;
using UnityEngine;

namespace BehTreeBrick.Conditrion
{
    [BehaviourButton("Combat/RichTargetByContainer")]
    public class RichTargetByContainer : RicherTarget
    {
        [SerializeField] private Unit _unit;
        [Min(0)][SerializeField] private float _distacne;
        private TargetContainer _target;

        private void Awake() => _target = _unit.MainDates.GetOrNull<TargetContainer>();

        public override bool IsRich() => Vector3.Distance(_target.TargetMove, _unit.transform.position) < _distacne;

        private void OnDrawGizmosSelected()
        {
            if(!_unit)
                return;
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_unit.transform.position, _distacne);
        }
    }
}