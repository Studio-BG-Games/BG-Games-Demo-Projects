using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.UI.UniversalThings
{
    public class IconLabel : MonoBehaviour
    {
        [SerializeField] private Image _image;

        public Image Image => _image;
        
        public void SetIcon(Sprite sprite) => _image.sprite = sprite;
        
        public void SetAlpha(float value)
        {
            var color = _image.color;
            color.a = Mathf.Clamp(value, 0, 1);
            _image.color = color;
        }
    }
}