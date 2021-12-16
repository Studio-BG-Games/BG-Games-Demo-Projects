using System;
using DG.Tweening;
using UnityEngine;

namespace Gameplay.UI.AnimUI
{
    public class FadeUI : AnimUIAbs
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private float _onValue;
        [SerializeField] private float _offValue;
        [SerializeField] private float _duration;

        private Tween _animAction;

        public override bool InProgress => _animAction != null;

        public override void On(Action callback)
        {
            _animAction = _canvasGroup.DOFade(_onValue, _duration).OnComplete(() =>
            {
                _animAction = null;
                callback?.Invoke();
            });
        }

        public override void Off(Action callback)
        {
            _animAction = _canvasGroup.DOFade(_offValue, _duration).OnComplete(() =>
            {
                _animAction = null;
                callback?.Invoke();
            });
        }

        public override void Complete() => _animAction.Complete();
    }
}