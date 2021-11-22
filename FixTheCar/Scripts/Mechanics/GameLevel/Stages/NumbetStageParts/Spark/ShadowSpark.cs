using System;
using UnityEngine;
using UnityEngine.UI;

namespace Mechanics.GameLevel.Stages.NumbetStageParts.Spark
{
    public class ShadowSpark : MonoBehaviour
    {
        public event Action<ShadowSpark> Completed;
        public bool IsCompleted { get; private set; }
        
        public TypeSpark Type => _type;
        [SerializeField] private TypeSpark _type;
        [SerializeField] private Image _shadow;
        [SerializeField] private Color _colorOff;
        [SerializeField] private Color _colorOn;

        private Spark _spark;

        private void OnEnable() => _shadow.color = _colorOn;

        private void OnDisable() => _shadow.color = _colorOff;

        public bool ApplaySpark(Spark spark)
        {
            if (!enabled)
                return false;
            _spark = spark;
            ChangeSpark(_spark);
            ChangeMe();
            Completed?.Invoke(this);
            return true;
        }

        private void ChangeMe()
        {
            _shadow.enabled = false;
            IsCompleted = true;
            enabled = false;
        }

        private void ChangeSpark(Spark spark)
        {
            spark.transform.SetParent(transform);
            spark.transform.localPosition = Vector3.zero;
            _spark.enabled = false;
        }

        public enum TypeSpark
        {
            Slim, med, fat
        }
    }
}