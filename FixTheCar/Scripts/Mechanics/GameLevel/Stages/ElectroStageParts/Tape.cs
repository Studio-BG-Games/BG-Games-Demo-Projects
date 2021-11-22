using System;
using System.Collections.Generic;
using DG.Tweening;
using Plugins.DIContainer;
using Services.Interfaces;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Mechanics.GameLevel.Stages.ElectroStageParts
{
    [RequireComponent(typeof(Animator))]
    public class Tape : MonoBehaviour, IDragHandler, IEndDragHandler
    {
        [SerializeField] private float _durationGoToBack;

        private Animator _animator;

        [DI] private IInput _input;

        private void Awake() => _animator = GetComponent<Animator>();

        public void OnDrag(PointerEventData eventData)
        {
            Vector3 position = Camera.main.ScreenToWorldPoint(_input.InputScreenPosition);
            position.z = 0;
            transform.position = position;
            RayCastToTapePlace(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            enabled = false;
            transform.DOLocalMove(Vector3.zero, _durationGoToBack).SetEase(Ease.Linear).OnComplete(() => enabled = true);
        }

        private void RayCastToTapePlace(PointerEventData eventData)
        {
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);
            foreach (RaycastResult result in results)
                if (result.gameObject.TryGetComponent<TapePlace>(out var place) && place.IsFixed==false)
                    FixPlace(place);
        }

        private void FixPlace(TapePlace place)
        {
            place.ApplayTape(this);
            RotateMe();
        }

        private void RotateMe()
        {
            _animator.SetTrigger("RotateMe");
        }
    }
}