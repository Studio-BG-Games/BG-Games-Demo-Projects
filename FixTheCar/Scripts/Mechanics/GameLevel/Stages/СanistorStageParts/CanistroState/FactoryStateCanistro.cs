using System;
using Plugins.DIContainer;
using UnityEngine;

namespace Mechanics.GameLevel.Stages.СanistorStageParts
{
    public class FactoryStateCanistro : MonoBehaviour
    {
        [SerializeField] private CanistorStage _canistorStage;
        [SerializeField] private BigCanistro _bigCanistro;

        private Player _player;
        private DiBox _diBox = DiBox.MainBox;

        private void Awake() => _canistorStage.GetPlayer += OnGetPlayer;

        private void OnGetPlayer(Player obj)
        {
            _canistorStage.GetPlayer -= OnGetPlayer;
            _player = obj;
        }

        public T GetNewState<T>(Canistro canistro) where T : CanistroState
        {
            CanistroState result = null;
            if (typeof(T)  == typeof(FuelState)) result = new FuelState(_bigCanistro, canistro);
            if (typeof(T) == typeof(EmptyState)) result = new EmptyState(canistro);
            if (typeof(T) == typeof(OilState)) result = new OilState(canistro, _player);
            if(result==null)
                throw new Exception("No type in factory to create");
            _diBox.InjectSingle(result);
            return result as T;
        }
    }
}