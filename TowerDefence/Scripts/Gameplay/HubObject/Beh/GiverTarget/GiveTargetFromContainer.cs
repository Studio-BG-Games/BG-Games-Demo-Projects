using System.Collections.Generic;
using Gameplay.Units;
using Plugins.HabObject;
using UnityEngine;

namespace Gameplay.HubObject.Beh.Attack
{
    [BehaviourButton("Combat/Attack/GiveTarget/From container")]
    public class GiveTargetFromContainer : GiveTarget
    {
        [SerializeField] private Unit _unit;

        public override List<HabObject> All()
        {
            var target = GetOne();
            var result = new List<HabObject>();
            if(target!=null && target!=_unit)
                result.Add(target);
            return result;
        }

        public override HabObject GetOne()
        {
            return _unit.MainDates.GetOrNull<TargetContainer>().TargetAttack;
        }
    }
}