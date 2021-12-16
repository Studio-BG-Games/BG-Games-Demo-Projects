using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.UI.Menu.Canvas
{
    public class ViewStringInfo_DefaultText : ViewStringInfo
    {
        [SerializeField]private Text _label;

        protected override void SetText(string text)
        {
            _label.text = text;
        }
    }
}