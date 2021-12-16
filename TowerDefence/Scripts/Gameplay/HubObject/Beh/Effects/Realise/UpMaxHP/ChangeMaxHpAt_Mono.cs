using Plugins.HabObject;
using UnityEngine;

namespace Gameplay.HubObject.Beh.Effects.Realise.UpMaxHP
{
    [BehaviourButton("Effects/ChangeMaxHpAt")]
    public class ChangeMaxHpAt_Mono : EffectMonoWarper
    {
        protected override IEffect _someEffect => changeMaxHpAt;
        [SerializeField] private ChangeMaxHpAt changeMaxHpAt;
    }
}