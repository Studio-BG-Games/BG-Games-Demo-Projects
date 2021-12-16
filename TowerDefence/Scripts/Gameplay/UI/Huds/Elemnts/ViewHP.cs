using System;
using Gameplay.HubObject.Beh;
using Gameplay.HubObject.Beh.Attributes;
using Gameplay.UI.Huds;
using UnityEngine;
using UnityEngine.UI;

namespace Temp
{
    [AddComponentMenu("Hud/hud element/View HP")]
    public class ViewHP : HudElement
    {
        [SerializeField] private Health _health;
        [SerializeField] private Image _bar;
        
        protected override void CustomAwake()
        {
            base.CustomAwake();
            _bar.fillAmount = _health.Current / (float)_health.RealMax;
            _health.NewValue += x => _bar.fillAmount = x;
        }
    }
}