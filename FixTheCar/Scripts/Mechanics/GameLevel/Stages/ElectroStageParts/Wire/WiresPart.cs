using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Plugins.DIContainer;
using Services.Interfaces;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Mechanics.GameLevel.Stages.ElectroStageParts.Wire
{
    public class WiresPart : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public event Action Fixed;
        public event Action Failed;
        public event Action Broked;
        
        [SerializeField] private WireShadow _wireShadow;
        [SerializeField] private Transform _pointOnPanel;

        [DI] private IInput _input;
        
        private float _durationReturn = 1;

        [DI]
        private void Init()
        {
            if(!_pointOnPanel) throw null;
            enabled = false;
        }

        public void Breake()
        {
            enabled = true;
            transform.position = _pointOnPanel.position;
            Broked?.Invoke();
        }
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            _wireShadow.Selected();
        }

        public void OnDrag(PointerEventData eventData)
        {
            Vector3 newPosition = Camera.main.ScreenToWorldPoint(_input.InputScreenPosition);
            newPosition.z = 0;
            transform.position = newPosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _wireShadow.Unselected();
            if (IsMyShadow(GetListShadows(eventData)))
            {
                transform.position = _wireShadow.transform.position;
                enabled = false;
                Fixed?.Invoke();
            }
            else
            {
                enabled = false;
                transform.DOMove(_pointOnPanel.transform.position, _durationReturn).OnComplete(() => enabled = true);
                Failed?.Invoke();
            }
        }

        private bool IsMyShadow(List<RaycastResult> getListShadows) => getListShadows.Count > 0;

        private List<RaycastResult> GetListShadows(PointerEventData eventData)
        {
            var list = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, list);
            var myShadow = list.Where(x => x.gameObject.GetComponent<WireShadow>()).Where(x => x.gameObject.GetComponent<WireShadow>() == _wireShadow).ToList();
            return myShadow;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, 1);
            if(!_pointOnPanel || !_wireShadow) return;
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(_wireShadow.transform.position, _pointOnPanel.position);
        }
    }
}