using System.Collections.Generic;
using Gameplay.HubObject.Beh.Damages;
using Gameplay.HubObject.Beh.Effects;
using Plugins.HabObject;
using UnityEngine;

namespace Gameplay.HubObject.Beh.Resistance
{
    [System.Serializable]
    public class IncludeResistance : IEffect
    {
        [SerializeField] private List<TypeDamage> _elementsToResistance;
        [SerializeField] private int _valueResistance;

        public IModificateData Make(HabObject habObject, IModificateData modificateData)
        {
            var data = (Damage) modificateData;
            if (data == null)
                return modificateData;
            
            List<ElementDagame> newElements = new List<ElementDagame>();
            data.GoOverElement(e =>
            {
                if(_elementsToResistance.Contains(e.TypeDamage))
                    newElements.Add(e.CloneWithNewValue(e.Value-_valueResistance));
                else
                    newElements.Add(e);
            });
            return data.CloneWithReplaceElement(newElements);
        }

        public IEffect Clone()
        {
            var clone = new IncludeResistance();
            clone._elementsToResistance = _elementsToResistance;
            clone._valueResistance = _valueResistance;
            return clone;
        }

        public int Priority => 5;
    }
}