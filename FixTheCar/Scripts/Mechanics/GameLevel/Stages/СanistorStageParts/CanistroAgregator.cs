using System;
using System.Collections.Generic;
using Plugins.DIContainer;
using UnityEngine;

namespace Mechanics.GameLevel.Stages.СanistorStageParts
{
    public class CanistroAgregator : MonoBehaviour
    {
        public event Action SomeCanistroActionStart;
        public event Action SomeCanistroActionFinish;
        
        [SerializeField] private FactoryCanistro _factoryCanistro;

        private List<Canistro> _canistros;
        
        [DI]
        private void Init() => _factoryCanistro.Created += OnCreatedCanistro;

        public void OnCreatedCanistro(List<Canistro> canistors)
        {
            _factoryCanistro.Created -= OnCreatedCanistro;
            _canistros = canistors;
            ChangeInteractTo(false);
            _canistros.ForEach(x =>
            {
                x.StartCaistroAction += StartActionCanistro;
                x.FinishCanistroAction += FinishActionCanistro;
            });
        }

        private void StartActionCanistro() => SomeCanistroActionStart?.Invoke();

        private void FinishActionCanistro() => SomeCanistroActionFinish?.Invoke();

        public void Show(float durationUnfade, Action callback) => _canistros.ForEach(x=>x.Show(durationUnfade, callback));

        public void Hide(float durationFade, Action callback) => _canistros.ForEach(x=>x.Hide(durationFade, callback));

        public void ChangeInteractTo(bool isActive) => _canistros?.ForEach(x=>x.enabled=isActive);

        private void OnDestroy()
        {
            _canistros?.ForEach(x =>
            {
                x.StartCaistroAction -= StartActionCanistro;
                x.FinishCanistroAction -= FinishActionCanistro;
            });
        }
    }
}