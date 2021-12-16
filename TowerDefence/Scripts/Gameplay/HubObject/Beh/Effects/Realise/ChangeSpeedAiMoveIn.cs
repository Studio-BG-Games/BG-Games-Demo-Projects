using Gameplay.HubObject.Beh.Attributes;
using Plugins.HabObject;
using UnityEngine;

namespace Gameplay.HubObject.Beh.Effects.Realise
{
    [System.Serializable]
    public class ChangeSpeedAiMoveIn : IEffect
    {
        [Min(0.1f)]public float _multiplyIn;

        public ChangeSpeedAiMoveIn(float multiplyAt) => _multiplyIn = multiplyAt;
        
        public IModificateData Make(HabObject habObject, IModificateData modificateData)
        {
            var data = modificateData as SpeedAIPath.ModificateData;
            if (data == null)
                return modificateData;
            data.Speed *= _multiplyIn;
            return data;
        }

        public IEffect Clone() => new ChangeSpeedAiMoveIn(_multiplyIn);

        public int Priority => 2;
    }
}