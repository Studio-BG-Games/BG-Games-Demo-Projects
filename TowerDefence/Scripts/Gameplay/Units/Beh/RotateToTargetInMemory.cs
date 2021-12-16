using UnityEngine;

namespace Gameplay.Units.Beh
{
    public class RotateToTargetInMemory : MonoBehaviour
    {
        [SerializeField] private Unit _unit;
        
        public void Rotate()
        {
            var target = _unit.MainDates.GetOrNull<TargetContainer>().TargetAttack;
            if(!target)
                return;
            var dir = (target.transform.position - _unit.transform.position).normalized;
            dir.y = 0;
            Quaternion lookRot = Quaternion.LookRotation(dir);
            _unit.transform.rotation = lookRot;
        }
    }
}