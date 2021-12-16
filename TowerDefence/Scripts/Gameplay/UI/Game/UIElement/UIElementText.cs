using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.UI.Game.UIElement
{
    public class UIElementText : UIElement
    {
        [SerializeField] private Text _label;

        public void Init(string text)
        {
            _label.text = text;
        }
    }
}