using System;
using PathCreation;
using UnityEngine;

namespace Mechanics.GameLevel.Stages.ElectroStageParts
{
    public class ElectroPath : MonoBehaviour
    {
        [SerializeField] private PathCreator _pathCreator;
        [SerializeField] private ElectroSpark _spark;
        
        public event Action Finished;
        public event Action Failed;

        private void Awake()
        {
            _spark.Finished += OnFinish;
            _spark.Fail += OnFail;
        }

        private void OnDestroy()
        {
            _spark.Finished -= OnFinish;
            _spark.Fail -= OnFail;
        }

        private void OnFail() => Failed?.Invoke();

        private void OnFinish() => Finished?.Invoke();

        public void StartMove() => _spark.Move(_pathCreator);
    }
}