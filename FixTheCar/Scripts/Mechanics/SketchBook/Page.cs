using System;
using System.Collections.Generic;
using DG.Tweening;
using Factories;
using Infrastructure.Configs;
using UnityEngine;

namespace Mechanics.SketchBook
{
    [RequireComponent(typeof(CanvasGroup), typeof(RectTransform))]
    public class Page : MonoBehaviour
    {
        public RectTransform RectTransform => _rectTransform;

        [SerializeField] private List<Sticker> _stickers;

        private CanvasGroup _canvasGroup;
        private RectTransform _rectTransform;
        
        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _rectTransform = GetComponent<RectTransform>();
        }

        public void Init(in List<ConfigLevel> levels, out List<ConfigLevel> usedConfigs)
        {
            usedConfigs = new List<ConfigLevel>();
            for (int i = 0; i < _stickers.Count; i++)
            {
                if(i>=levels.Count)
                    break;
                _stickers[i].Init(levels[i]);
                usedConfigs.Add(levels[i]);
            }
        }

        public void Show(float duration, Action callback = null) => 
            _canvasGroup.DOFade(1, duration).OnComplete(()=>
            {
                _canvasGroup.interactable = true;
                callback?.Invoke();
            });

        public void Hide(float duration, Action callback = null) => 
            _canvasGroup.DOFade(0, duration).OnStart(()=>_canvasGroup.interactable = false).OnComplete(()=>callback?.Invoke());
    }
}