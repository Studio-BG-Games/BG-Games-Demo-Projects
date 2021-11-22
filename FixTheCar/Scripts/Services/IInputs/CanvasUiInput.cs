using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Services.IInputs
{
    public class CanvasUiInput : MonoBehaviour
    {
        public ButtonInputUI RightButton => _rightButton;
        public ButtonInputUI LeftButton => _leftButton;
        [SerializeField] private ButtonInputUI _rightButton;
        [SerializeField] private ButtonInputUI _leftButton;

        [SerializeField] private CanvasGroup _canvasGroup;

        public void Show() => _canvasGroup.alpha = 1;

        public void Hide() => _canvasGroup.alpha = 0;
    }
}