using System;
using DG.Tweening;
using Plugins.Interfaces;
using UnityEngine;

namespace Services
{
    public class Curtain : MonoBehaviour, ICurtain
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private float _fadeDuration;

        public void Fade(Action callback) => Fade(callback, _fadeDuration);
        public void Fade(Action callback, float duration) => _canvasGroup.DOFade(1, duration).OnComplete(()=>callback.Invoke());

        public void Unfade() => Unfade(_fadeDuration);
        public void Unfade(float duration) => _canvasGroup.DOFade(0, duration);

        public void Transit(Action callback)
        {
            Fade(() =>
            {
                callback?.Invoke(); 
                Unfade();
            });
        }
    }
}