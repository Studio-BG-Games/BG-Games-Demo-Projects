using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.UI.Game.UIElement
{
    [RequireComponent(typeof(Button), typeof(LayoutElement))]
    public class UIButton : UIElement
    {
        private Button _button;

        public Button Button => _button != null?_button:_button=GetComponent<Button>();
    }
}