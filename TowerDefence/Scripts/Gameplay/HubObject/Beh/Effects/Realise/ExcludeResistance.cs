using System.Collections.Generic;
using Gameplay.HubObject.Beh.Damages;
using Gameplay.HubObject.Beh.Effects;
using Plugins.HabObject;
using UnityEngine;

namespace Gameplay.HubObject.Beh.Resistance
{
    [System.Serializable]
    public class ExcludeResistance : IEffect
    {
        [SerializeField] private List<TypeDamage> _elementsToNotResistance;
        [SerializeField] private int _valueResistance;
        
        public IModificateData Make(HabObject habObject, IModificateData modificateData)
        {
            var data = modificateData as Damage;
            if (data == null)
                return data;

            List<ElementDagame> newElements = new List<ElementDagame>();
            data.GoOverElement(e =>
            {
                if(!_elementsToNotResistance.Contains(e.TypeDamage))
                    newElements.Add(e.CloneWithNewValue(e.Value-_valueResistance));
                else
                    newElements.Add(e);
            });
            return data.CloneWithReplaceElement(newElements);
        }

        public IEffect Clone()
        {
            var clone = new ExcludeResistance();
            clone._elementsToNotResistance = _elementsToNotResistance;
            clone._valueResistance = _valueResistance;
            return clone;
        }

        public int Priority => 5;
    }
}