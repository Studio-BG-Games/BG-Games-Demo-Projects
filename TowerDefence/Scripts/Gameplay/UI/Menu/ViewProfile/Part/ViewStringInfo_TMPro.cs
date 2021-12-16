using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.UI.Menu.Canvas
{
    public class ViewStringInfo_TMPro : ViewStringInfo
    {
        [SerializeField]private TextMeshProUGUI _label;
        protected override void SetText(string text)
        {
            _label.text = text;
        }
    }
}