using System;
using DG.Tweening;
using UnityEngine;

namespace Mechanics.Garage
{
    [RequireComponent(typeof(Car))]
    public class CarSizer : MonoBehaviour
    {
        [SerializeField] private Vector3 _startSize;
        [SerializeField] private Vector3 _endSize;
        [SerializeField] private float _durationChangeSize;

        private Car _car;
        private Tween _tween;
        
        private void Awake()
        {
            transform.localScale = _startSize;
            _car = GetComponent<Car>();
            _car.Selected += OnSelected_MakeBig;
            _car.Unselected += OnUnselected_MakeSmall;
        }

        [ContextMenu("TO SMALL")]
        private void OnUnselected_MakeSmall()
        {
            KillPrevTween();
            _tween = transform.DOScale(_startSize, _durationChangeSize).SetEase(Ease.Linear);
        }

        [ContextMenu("TO BIG")]
        private void OnSelected_MakeBig()
        {
            KillPrevTween();
            _tween = transform.DOScale(_endSize, _durationChangeSize).SetEase(Ease.Linear);
        }

        private void KillPrevTween()
        {
            if (_tween != null)
                _tween.Kill();
        }
    }
}