using System;
using System.Collections.Generic;
using Mechanics.GameLevel.Stages.NumbetStageParts.Spark;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Mechanics.GameLevel.Stages.NumbetStageParts
{
    public class Engine : MonoBehaviour
    {
        public event Action NewStage;
        public event Action Completed;
        
        [SerializeField] private Transform[] _pointSpark;
        [SerializeField] private ShadowSpark[] _templates;

        private Queue<ShadowSpark> _sparks;
        private ShadowSpark _current;

        public ShadowSpark.TypeSpark CurrentSparkType
        {
            get
            {
                if (_current)
                    return _current.Type;
                throw new Exception("Engine is ready");
            }
        } 

        public void GenerateShadow() => _sparks = Generate();

        private Queue<ShadowSpark> Generate()
        {
            Queue<ShadowSpark> result = new Queue<ShadowSpark>();            
            foreach (var point in _pointSpark)
            {
                var template = _templates[Random.Range(0, _templates.Length)];
                var instance = Instantiate(template, point);
                instance.transform.localPosition = Vector3.zero;
                instance.enabled = false;
                instance.Completed += OnCompletedSpark;
                result.Enqueue(instance);
            }

            _current = result.Dequeue();
            _current.enabled = true;
            NewStage?.Invoke();
            return result;
        }

        private void OnCompletedSpark(ShadowSpark spark)
        {
            if (_sparks.Count == 0)
            {
                Completed?.Invoke();
                return;
            }
            _current.enabled = false;
            _current = _sparks.Dequeue();
            _current.enabled = true;
            NewStage?.Invoke();
        }
    }
}