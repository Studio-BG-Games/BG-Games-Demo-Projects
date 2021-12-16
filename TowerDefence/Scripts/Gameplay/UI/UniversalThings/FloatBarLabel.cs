using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.UI.UniversalThings
{
    public class FloatBarLabel : MonoBehaviour
    {
        [SerializeField] private Image _image;

        public Image Image => _image;

        public void SetNewValue(float value) => _image.fillAmount = Clamp(value);

        public void SetNewValue(float current, float max) => SetNewValue(current / max);

        private float Clamp(float value) => Mathf.Clamp(value, 0, 1);
    }
}