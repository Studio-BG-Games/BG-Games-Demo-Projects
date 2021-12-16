using Gameplay.HubObject.Beh.Effects.Realise.UpMaxHP;
using UnityEngine;

namespace Gameplay.HubObject.Beh.Effects.Realise
{
    [BehaviourButton("Effects/ChangeSpeedAiMoveIn")]
    public class ChangeSpeedAiMoveIn_Mono : EffectMonoWarper
    {
        protected override IEffect _someEffect => _changeSpeedAi;
        [SerializeField] private ChangeSpeedAiMoveIn _changeSpeedAi;
    }
}