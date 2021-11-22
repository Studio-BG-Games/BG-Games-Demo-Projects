using System;
using DG.Tweening;
using Plugins.Interfaces;
using UnityEngine;

namespace Services.Curtains
{
    public class CanvasCurtain : Curtain
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [Min(0.01f)][SerializeField] private float _duration;
        [SerializeField] private Ease _ease;

        public override event Action Unfaded;

        public override void Fade(Action callback) => _canvasGroup.DOFade(1, _duration).SetEase(_ease).OnComplete(()=>callback?.Invoke());

        public override void Fade(Action callback, float duration) => _canvasGroup.DOFade(1, duration).SetEase(_ease).OnComplete(()=>callback?.Invoke());

        public override void Unfade() => _canvasGroup.DOFade(0, _duration).SetEase(_ease).OnComplete(()=>Unfaded?.Invoke());

        public override void Transit(Action callback) => Fade(() => { callback?.Invoke(); Unfade();});
    }
}