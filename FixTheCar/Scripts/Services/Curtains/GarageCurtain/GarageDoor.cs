using System;
using System.Collections;
using DG.Tweening;
using Factories;
using UnityEngine;

namespace Services.Curtains.GarageCurtain
{
    public class GarageDoor  : MonoBehaviour
    {
        [SerializeField] private bool _toRight;
        
        private RectTransform _rectTransform;
        private Vector2 _centerPoint;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            InitSize();
            _rectTransform.anchoredPosition = OpenPoint();
        }

        public void Open(float duration) 
            => _rectTransform.DOAnchorPos(OpenPoint(), duration);

        public void Close(float duration, Action callback = null) 
            => _rectTransform.DOAnchorPos(_centerPoint, duration).OnComplete(() => callback?.Invoke());

        private void InitSize()
        {
            SetSize();
            _centerPoint = GetCenterPoint();
        }

        private Vector2 GetCenterPoint() => 
            _toRight ? new Vector2(Screen.width / 4, 0) : new Vector2(Screen.width / -4, 0);

        private void SetSize() => 
            _rectTransform.sizeDelta = new Vector2(Screen.width / 2 + 1, Screen.height);

        private Vector3 OpenPoint() 
            => new Vector3(_centerPoint.x * 3, 0, 0);
    }
}