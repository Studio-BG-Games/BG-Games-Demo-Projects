using UnityEngine;

namespace Gameplay.UI.Menu
{
    public class InstantChangeCanvas : ButtonChangeCanvas
    {
        public override void Change()
        {
            ((RectTransform)Current.RootPanel.transform).anchoredPosition=Vector2.zero;
            Current.gameObject.SetActive(false);
            var newCanvas = Choise.Get();
            newCanvas.gameObject.SetActive(true);
            ((RectTransform)newCanvas.RootPanel.transform).anchoredPosition=Vector2.zero;
        }
    }
}