using Gameplay.HubObject.Beh.Effects.Realise.UpMaxHP;
using UnityEngine;

namespace Gameplay.HubObject.Beh.Effects.Realise
{
    [BehaviourButton("Effects/Zero in come damage")]
    public class ZeroInComeDamageWithChance_Mono : EffectMonoWarper
    {
        protected override IEffect _someEffect =>  _effetct;

        [SerializeField] private ZeroInComeDamageWithChance _effetct;
    }
}