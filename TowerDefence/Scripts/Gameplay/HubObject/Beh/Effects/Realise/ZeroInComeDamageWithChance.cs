using System.Collections.Generic;
using Gameplay.HubObject.Beh.Damages;
using Plugins.HabObject;
using UnityEngine;

namespace Gameplay.HubObject.Beh.Effects.Realise
{
    [System.Serializable]
    public class ZeroInComeDamageWithChance : IEffect
    {
        [Range(0,1f)][SerializeField] private float _chance;

        public ZeroInComeDamageWithChance(float chance) => _chance = chance;
        
        public IModificateData Make(HabObject habObject, IModificateData modificateData)
        {
            var data = modificateData as Damage;
            if (data == null)
                return modificateData;
            if (data.HabObject == habObject)
                return modificateData;
            if (Random.Range(0, 1f) > _chance)
                return modificateData;
            var newElemetn = new List<ElementDagame>();
            data.GoOverElement(x=>newElemetn.Add(x.CloneWithNewValue(0)));
            return data.CloneWithReplaceElement(newElemetn);
        }

        public IEffect Clone() => new ZeroInComeDamageWithChance(_chance);

        public int Priority => 15;
    }
}