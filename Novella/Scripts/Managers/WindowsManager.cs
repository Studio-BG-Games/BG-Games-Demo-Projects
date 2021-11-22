using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Scripts.UISystem
{
    public class WindowsManager : SceneSingleton<WindowsManager>
    {
#pragma warning disable 0649
        [SerializeField] private Transform screensContainer;
        [SerializeField] private Transform windowsContainer;
        [SerializeField] private Transform effectsContainer;
#pragma warning restore 0649


        [Header("Other References")]
#pragma warning disable 0649
        [SerializeField]
        private Canvas _canvas;

        [SerializeField] CanvasGroup fadeGroup;
        [SerializeField] private CanvasScaler canvasScaler;
#pragma warning restore 0649


        public Transform ScreensContainer => screensContainer;

        public Transform WindowsContainer => windowsContainer;

        public Transform EffectsContainer => effectsContainer;

        public Canvas Canvas => _canvas;


        public bool IsWindowOnTop(WindowController windowController)
        {
            return windowsContainer.childCount - 1 == windowController.transform.GetSiblingIndex();
        }

        public void CloseCurrentScreen()
        {
            if (screensContainer.childCount > 0)
                screensContainer.GetChild(0).GetComponent<WindowController>().CloseWindow();
        }

        public void CloseTopWindow()
        {
            windowsContainer.GetChild(windowsContainer.childCount - 1).GetComponent<WindowController>().CloseWindow();
        }

        public void CloseAllWindows()
        {
            for (var i = 0; i < windowsContainer.childCount; i++)
            {
                windowsContainer.GetChild(i).GetComponent<WindowController>().CloseWindow();
            }
        }

        public void ChangeFade(float fadeValue, float delay = 0f, float fadeTime = 0.5f, System.Action action = null)
        {
            fadeGroup.DOFade(fadeValue, fadeTime).SetDelay(delay).OnComplete(() => { action?.Invoke(); });
        }

        public T SearchForWindow<T>() where T : WindowController
        {
            for (var i = 0; i < windowsContainer.childCount; i++)
            {
                if (windowsContainer.GetChild(i).GetComponent<T>())
                {
                    return windowsContainer.GetChild(i).GetComponent<T>();
                }
            }

            return null;
        }
        
        public T SearchForScreen<T>() where T : WindowController
        {
            for (var i = 0; i < screensContainer.childCount; i++)
            {
                if (screensContainer.GetChild(i).GetComponent<T>())
                {
                    return screensContainer.GetChild(i).GetComponent<T>();
                }
            }

            return null;
        }

        public T CreateScreen<T>(string root = "") where T : WindowController
        {
            var screen = SearchForScreen<T>();
            if (screen != null)
            {
                screen.UpdateView();
                return screen;
            }

            CloseCurrentScreen();
            var windowPrefab = Resources.Load<GameObject>("Screens/" + root + typeof(T).Name);
            Debug.Log(windowPrefab);
            
            screen = Instantiate(windowPrefab, screensContainer).GetComponent<T>();
            screen.OpenWindow();

            return screen;
        }

        public T CreateWindow<T>(string root = "") where T : WindowController
        {
            var window = SearchForWindow<T>();
            if (window == null)
            {
                var windowPrefab = Resources.Load<GameObject>("Windows/" + root + typeof(T).Name);
                window = Instantiate(windowPrefab, windowsContainer).GetComponent<T>();
                window.OpenWindow();
            }
            else
            {
                MakeWindowOnTop(window);
                window.UpdateView();
            }

            return window;
        }

        public void MakeWindowOnTop(WindowController window)
        {
            window.transform.SetSiblingIndex(windowsContainer.childCount);
        }

        //Convert to canvas position
        public Vector3 UnscaleEventDelta(Vector3 vec)
        {
            var referenceResolution = canvasScaler.referenceResolution;
            var currentResolution = new Vector2(Screen.width, Screen.height);

            var widthRatio = currentResolution.x / referenceResolution.x;
            var heightRatio = currentResolution.y / referenceResolution.y;
            var ratio = Mathf.Lerp(widthRatio, heightRatio, canvasScaler.matchWidthOrHeight);

            return vec / ratio;
        }
    }
}