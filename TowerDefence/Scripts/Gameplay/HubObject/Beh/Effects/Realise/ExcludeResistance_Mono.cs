using Gameplay.HubObject.Beh.Effects;
using Gameplay.HubObject.Beh.Effects.Realise.UpMaxHP;
using UnityEngine;

namespace Gameplay.HubObject.Beh.Resistance
{
    [BehaviourButton("Effects/ExcludeResistance")]
    public class ExcludeResistance_Mono : EffectMonoWarper
    {
        protected override IEffect _someEffect => _excludeResistance;
        [SerializeField] private ExcludeResistance _excludeResistance;
    }
}