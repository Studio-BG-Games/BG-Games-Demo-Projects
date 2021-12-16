using Gameplay.HubObject.Beh.Attributes;
using Plugins.HabObject;
using UnityEngine;

namespace Gameplay.HubObject.Beh.Effects.Realise.UpMaxHP
{
    [System.Serializable]
    public class ChangeMaxHpAt : IEffect
    {
        [SerializeField] private int _value;
        
        public IModificateData Make(HabObject habObject, IModificateData modificateData)
        {
            if (!(modificateData is Health.ModificateData))
                return modificateData;
            var hData = modificateData as Health.ModificateData;

            var prevMax = hData.Max;
            hData.Max += _value;
            hData.Current = Mathf.RoundToInt((((float)hData.Max / prevMax) * hData.Current));
            Debug.Log($"{(float)hData.Max}/{prevMax} * {hData.Current} = {hData.Current}");

            return hData;
        }
        
        public IEffect Clone()
        {
            var result = new ChangeMaxHpAt();
            result._value = _value;
            return result;
        }

        public int Priority => 1;
    }
}