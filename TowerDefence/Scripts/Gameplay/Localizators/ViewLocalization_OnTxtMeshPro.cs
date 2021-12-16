using TMPro;
using UnityEngine;

namespace Gameplay.Localizators
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class ViewLocalization_OnTxtMeshPro : ViewTextByLocalizator
    {
        private TextMeshProUGUI _label;

        protected override void CustomAwake() => _label = GetComponent<TextMeshProUGUI>();

        protected override void SetText(string text) => _label.text = text;
    }
}