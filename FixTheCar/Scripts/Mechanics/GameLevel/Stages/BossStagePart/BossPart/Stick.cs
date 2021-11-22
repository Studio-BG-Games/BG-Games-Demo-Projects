using System;
using DG.Tweening;
using Mechanics.GameLevel.Stages.BossStagePart.Interfaces;
using Plugins.DIContainer;
using UnityEngine;

namespace Mechanics.GameLevel.Stages.BossStagePart
{
    public class Stick : MonoBehaviour
    {
        public Transform PointKlash => _pointKlash;

        [SerializeField] private Transform _pointKlash;
        [SerializeField] private float _fixedAngelForRotateToTarget = 90;
        
        private Vector3 _zeroScale;
        private Vector3 _zeroRotate;
        private Tween _moveTween;
        private Tween _rotateTween;
        
        [DI]
        private void Init()
        {
            _zeroScale = transform.localScale;
            _zeroRotate = transform.localEulerAngles;
        }
        
        public void MakeShort(float duration, Action callback=null)
        {
            _moveTween?.Complete();
            _moveTween = transform.DOScale(_zeroScale, duration).OnComplete(() => callback?.Invoke());
        }

        public void RotateToZero(float duration, Action callback = null)
        {
            _rotateTween?.Complete();
            _rotateTween = transform.DOLocalRotate(_zeroRotate, duration);
        }

        public void LongTo(Transform target, float duration, Action callback = null)
        {
            _moveTween?.Complete();
            Vector3 newScale = _zeroScale;
            newScale.x = Vector3.Distance(transform.position, target.position);
            _moveTween = transform.DOScale(newScale, duration).OnComplete(() => callback?.Invoke());
        }

        public void Rotate(Transform randomTarget, float duration, Action action = null)
        {
            _rotateTween?.Complete();
            _rotateTween = transform.DOLocalRotateQuaternion(GetQuaternionForRotate(randomTarget), duration).OnComplete(() => action?.Invoke());
        }

        private Quaternion GetQuaternionForRotate(Transform randomTarget)
        {
            var diraction = randomTarget.position - transform.position;
            //математика сам которую слабо понимаю
            // Источник - https://xgm.guru/p/unity/117047
            //.....
            float angle = Mathf.Atan2(-diraction.x, diraction.y) * Mathf.Rad2Deg;
            angle += _fixedAngelForRotateToTarget;
            //.....
            return Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
}