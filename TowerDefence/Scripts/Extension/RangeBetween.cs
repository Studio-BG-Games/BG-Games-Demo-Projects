using System;
using UnityEngine;

namespace Extension
{
    [Serializable]
    public class RangeBetween
    {
        [SerializeField] private float _min;
        [SerializeField] private float _max;

        public RangeBetween(int min, int max) : this((float)min, (float)max) { }

        public RangeBetween(float min, float max)
        {
            if(_min>=max)
                throw new Exception("Wrong Range. Min more then MAX!");
            _min = min;
            _max = max;
        }

        public float Get(float normal)
        {
            normal = Mathf.Clamp(normal, 0, 1);
            //Вопрсы? https://www.desmos.com/calculator/xgbxmuc4yo - Глянь график
            return ((_max - _min) * normal) + _min;
        }
    }
}