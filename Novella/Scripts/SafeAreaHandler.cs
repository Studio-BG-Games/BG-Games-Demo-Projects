using UnityEngine;

namespace Scripts.UISystem
{
    public class SafeAreaHandler : MonoBehaviour
    {
#pragma warning disable 0649
        [SerializeField] private RectTransform SafeRect;
#pragma warning restore 0649


        private Rect lastSafeArea = Rect.zero;
        private Canvas canvas;

        private void Update()
        {
            if (lastSafeArea != Screen.safeArea)
            {
                lastSafeArea = Screen.safeArea;
                ApplySafeArea();
            }
        }

        void OnEnable()
        {
            canvas = WindowsManager.Instance.Canvas;
            lastSafeArea = Screen.safeArea;
            ApplySafeArea();
        }

        void ApplySafeArea()
        {
            if (SafeRect == null)
                return;

            Rect safeArea = Screen.safeArea;

            Vector2 anchorMin = safeArea.position;
            Vector2 anchorMax = safeArea.position + safeArea.size;
            var pixelRect = canvas.pixelRect;
            anchorMin.x /= pixelRect.width;
            anchorMin.y /= pixelRect.height;
            anchorMax.x /= pixelRect.width;
            anchorMax.y /= pixelRect.height;

            SafeRect.anchorMin = anchorMin;
            SafeRect.anchorMax = anchorMax;
        }
    }
}