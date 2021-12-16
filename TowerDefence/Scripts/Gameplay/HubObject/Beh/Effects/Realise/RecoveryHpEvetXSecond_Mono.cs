using Gameplay.HubObject.Beh.Effects.Realise.UpMaxHP;
using UnityEngine;

namespace Gameplay.HubObject.Beh.Effects.Realise
{
    [BehaviourButton("Effects/RecoveryHpEvetXSecond")]
    public class RecoveryHpEvetXSecond_Mono : EffectMonoWarper
    {
        protected override IEffect _someEffect => _recoveryHp;
        [SerializeField] private RecoveryHpEverXSecond _recoveryHp;
    }
}