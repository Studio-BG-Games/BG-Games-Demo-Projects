using Plugins.HabObject.GeneralProperty;
using UnityEngine;

namespace Gameplay.Units.Data
{
    [DisallowMultipleComponent]
    public class TypeMonster : UnitProperty
    {
        public Fraction Frac;
        
        public enum Fraction
        {
            Peopel, animal, ork, deadPeopel
        }
    }
}