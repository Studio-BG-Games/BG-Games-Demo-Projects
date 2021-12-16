using System;
using DefaultNamespace.Infrastructure.Data;
using Plugins.DIContainer;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Gameplay.UI.Menu
{
    public class FeedbackPanel : MonoBehaviour
    {
        [DI] private IActionFireBase _actionFireBase;

        [SerializeField] private UnityEvent _onSend;
        
        [SerializeField] private InputField _inputField;
        [SerializeField] private Button _buttonSend;
        [Min(0)][SerializeField] private int _minCharToAcseeSend;

        private void Awake()
        {
            _buttonSend.interactable = false;
            _inputField.onValueChanged.AddListener(OnChangeField);
            _buttonSend.onClick.AddListener(OnSend);
        }

        private void OnSend()
        {
            _actionFireBase.SendFeedBack(_inputField.text);
            _buttonSend.interactable = false;
            _inputField.text = string.Empty;
            _onSend.Invoke();
        }

        private void OnChangeField(string arg0)
        {
            _buttonSend.interactable = !string.IsNullOrWhiteSpace(arg0) && arg0.Length >= _minCharToAcseeSend;
        }
    }
}