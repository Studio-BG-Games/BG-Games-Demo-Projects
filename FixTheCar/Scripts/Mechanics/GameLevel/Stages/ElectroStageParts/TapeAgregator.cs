using System;
using UnityEngine;

namespace Mechanics.GameLevel.Stages.ElectroStageParts
{
    public class TapeAgregator : MonoBehaviour
    {
        public event Action SomeTapeFixed;
        
        private TapePlace[] _tapePlace;

        private void Awake()
        {
            _tapePlace = GetComponentsInChildren<TapePlace>();
            foreach (var place in _tapePlace) place.Fixed += OnFixed;
        }

        public bool IsAllTapeFixed()
        {
            foreach (var place in _tapePlace)
                if (!place.IsFixed)
                    return place.IsFixed;
            return true;
        }
        
        private void OnFixed() => SomeTapeFixed?.Invoke();

        private void OnDestroy()
        {
            foreach (var place in _tapePlace) place.Fixed -= OnFixed;
        }
    }
}