using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Gameplay.UI.Game.Canvas
{
    public class LayoutCanvasWithPoint : CustomCanvas
    {
        [SerializeField] private List<Elelment> _elelments;
        [SerializeField] private RectTransform _panel;
        [SerializeField, Min(0)] private float _sizeElementForView = 60;

        public Elelment GetElement(Point point) => _elelments.First(x => x.PointName == point);
        private Coroutine _movePanel;
        private Camera _camera;

        private void Awake() => _camera =Camera.main;

        public void AttachTo(Transform tranm)
        {
            if (_movePanel != null)
            {
                StopCoroutine(_movePanel);
                _movePanel = null;
            }
            _movePanel = StartCoroutine(MoveBy(tranm));
        }

        private IEnumerator MoveBy(Transform tranm)
        {
            while (true)
            {
                var screenPoint = _camera.WorldToScreenPoint(tranm.position);
                _panel.anchoredPosition = screenPoint;
                yield return new WaitForEndOfFrame();    
            }
        }

        public void AddContent(Transform rectTransform, Point point)
        {
            rectTransform.SetParent(GetElement(point).Transform);
            rectTransform.localPosition = Vector3.zero;
        }

        public void RemoveAllContent()
        {
            foreach (var elelment in _elelments)
            {
                foreach (Transform transform1 in elelment.Transform.GetComponentsInChildren<Transform>().Except(new List<Transform>(){elelment.Transform}))
                {
                    if(transform1)
                        Destroy(transform1.gameObject);
                } 
            }
            if (_movePanel != null)
            {
                StopCoroutine(_movePanel);
                _movePanel = null;
            }
        }

        private void OnDrawGizmosSelected()
        {
            if(Application.isPlaying)
                return;
            Gizmos.color = Color.cyan;
            foreach (var elelment in _elelments)
            {
                if(elelment.Transform)
                    Gizmos.DrawWireSphere(elelment.Transform.position, _sizeElementForView);
            }
        }

        public enum Point
        {
            First, Second, Third, Fourth
        }

        [System.Serializable]
        public class Elelment
        {
            public Point PointName;
            public Transform Transform;
        }
    }
}