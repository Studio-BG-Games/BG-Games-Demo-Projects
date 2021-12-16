﻿using Gameplay.HubObject.Beh.Effects;
using Gameplay.HubObject.Beh.Effects.Realise.UpMaxHP;
using UnityEngine;

namespace Gameplay.HubObject.Beh.Resistance
{
    [BehaviourButton("Effects/IncludeResistance")]
    public class IncludeResistance_Mono : EffectMonoWarper
    {
        protected override IEffect _someEffect => _includeResistance;
        [SerializeField] private IncludeResistance _includeResistance;
    }
}