using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Gameplay.UI.Huds
{
    [AddComponentMenu("Hud/Hud main")][RequireComponent(typeof(RotateToMainCamera))]
    public class Hud : MonoBehaviour
    {
        private List<HudElement> _hudElements;

        private void Awake() => _hudElements = GetComponentsInChildren<HudElement>().ToList();

        private void Start() => AllFadeTo(0, ChangeStateToHudElement.TurnOff);

        public void AllFadeTo(float timefade, ChangeStateToHudElement stateTo) => _hudElements.ForEach(x => x.FadeTo(timefade, stateTo));

        public void AllFateTo(ChangeStateToHudElement stateTo) => _hudElements.ForEach(x => x.FadeTo(stateTo));

        public void AllFadeToInTime(float time, float timeFade, ChangeStateToHudElement changeStateTo) =>
            _hudElements.ForEach(x => x.FadeToInTime(time, timeFade, changeStateTo));
        
        public void AllFadeToInTime(float time, ChangeStateToHudElement changeStateTo) =>
            _hudElements.ForEach(x => x.FadeToInTime(time, changeStateTo));
    }
}