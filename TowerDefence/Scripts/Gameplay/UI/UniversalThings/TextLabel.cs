using TMPro;
using UnityEngine;

namespace Gameplay.UI.UniversalThings
{
    public class TextLabel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _labelText;

        public TextMeshProUGUI LabelText => _labelText;

        public void SetText(string value) => _labelText.text = value;
    }
}