using Gameplay.Units;
using Plugins.HabObject;
using UnityEngine;

namespace Gameplay
{
    [BehaviourButton("Other/Unit selector")]
    public class UnitSelector : SelectorHabObject
    {
        private Unit Unit => _unit ? _unit : _unit = GetComponentInParent<Unit>();
        private Unit _unit;
        
        public override HabObject Get() => Unit;
    }
}