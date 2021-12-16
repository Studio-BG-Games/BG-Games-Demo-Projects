using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Localizators
{
    [RequireComponent(typeof(Text))]
    public class ViewLocalizationOnDefaultText : ViewTextByLocalizator
    {
        private Text _label;

        protected override void CustomAwake() => _label = GetComponent<Text>();

        protected override void SetText(string text) => _label.text = text;
    }
}