using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Mechanics.GameLevel.Stages.СanistorStageParts
{
    public class BigCanistro : MonoBehaviour
    {
        public event Action Finished;

        [SerializeField] private List<Segment> _segments = new List<Segment>();

        private void Awake() => _segments.ForEach(x=>x.FillTo(0,0.000001f));

        public void IncreseAndCallIfNotFinished(float duration, Action callback)
        {
            var segment = _segments[0];
            _segments.Remove(segment);
            if(_segments.Count>0) segment.FillTo(1, duration, () => callback?.Invoke());
            else segment.FillTo(1, duration, () => Finished?.Invoke());
        }

        [System.Serializable]
        private class Segment
        {
            [SerializeField] private Image _image;
            
            public void FillTo(float target, float duration, Action callback = null) 
                => _image.DOFillAmount(target, duration).OnComplete(() => callback?.Invoke());
        }
    }
}