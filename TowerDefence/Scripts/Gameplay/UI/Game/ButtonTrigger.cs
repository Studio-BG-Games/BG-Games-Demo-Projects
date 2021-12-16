using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Gameplay.UI.AnimUI;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.UI.Game
{
    [RequireComponent(typeof(Button))]
    public class ButtonTrigger : MonoBehaviour
    {
        public event Action ActionOnEnable;
        public event Action ActionOnDisable;
        public event Action Click;

        public bool interactable
        {
            get => Button.interactable;
            set => Button.interactable = value;
        }

        [SerializeField] private Vector3 _selectedScale;
        [SerializeField] private float _duration;
        [SerializeField] private AnimUIAbs _animUiAbs;

        public bool IsEnable => _isEnable;
        private bool _isEnable;
        private Vector3 _nornalSclae;
        private Button _button;
        private Button Button => _button != null ? _button : _button = GetComponent<Button>();

        private void Awake()
        {
            _nornalSclae = transform.localScale;
            ChangeStateTo(_isEnable, false);
            Button.onClick.AddListener(()=>Click?.Invoke());
            Button.onClick.AddListener(()=>ChangeStateTo(!_isEnable));
        }

        public void ChangeStateTo(bool newIsEnable) => ChangeStateTo(newIsEnable, true);

        public void ForcedChangeState(bool toChnageState, bool action)
        {
            _animUiAbs.Complete();
            ChangeStateTo(toChnageState, action);
        }

        public void ChangeStateTo(bool newIsEnable, bool invokeAction)
        {
            if(_isEnable==newIsEnable || _animUiAbs.InProgress)
                return;
            if(invokeAction)
                InvokeAction(newIsEnable);
            if(newIsEnable) _animUiAbs.On(()=>_isEnable=newIsEnable);
            else _animUiAbs.Off(()=>_isEnable=newIsEnable);
        }

        private void InvokeAction(bool isEnable)
        {
            if (isEnable) ActionOnEnable?.Invoke();
            else ActionOnDisable?.Invoke();
        }
    }
}