using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.UI.Game.UIElement
{
    [RequireComponent(typeof(LayoutElement))]
    public class UIElement : MonoBehaviour
    {
        public UIElement UiElement => _uiElement ? _uiElement : _uiElement = GetComponent<UIElement>();
        private UIElement _uiElement;
    }
}