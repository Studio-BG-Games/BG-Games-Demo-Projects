using System;
using DG.Tweening;
using Interface;
using UnityEngine;
using UnityEngine.UI;

namespace Services
{
    public class CurtainProgress : MonoBehaviour, ICurtainProgress
    {
        [SerializeField] private Image _bar;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private float _fadeDuration;

        public void SetProgress(float normalProgres)
        {
            _bar.fillAmount = normalProgres;
        }

        public void Fade(Action callback) => Fade(callback, _fadeDuration);
        public void Fade(Action callback, float duration) => _canvasGroup.DOFade(1, duration).OnComplete(()=>callback.Invoke());

        public void Unfade() => Unfade(_fadeDuration);
        public void Unfade(float duration) => _canvasGroup.DOFade(0, duration).OnComplete(()=>SetProgress(0));

        public void Transit(Action callback)
        {
            Fade(() =>
            {
                callback?.Invoke();
                SetProgress(1);
                Unfade();
            });
        }
    }
}