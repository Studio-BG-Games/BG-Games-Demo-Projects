using System;
using System.Collections.Generic;
using Mechanics.GameLevel.Stages.ElectroStageParts.Machines;
using Plugins.DIContainer;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Mechanics.GameLevel.Stages.СanistorStageParts
{
    public class FactoryCanistro : MonoBehaviour
    {
        public event Action<List<Canistro>> Created;

        [SerializeField] private Canistro _canistroTemplate;
        [SerializeField] private List<Transform> _pointForCanistor = new List<Transform>();
        [SerializeField] private FactoryStateCanistro _factoryStateCanistro;
        
        private List<Canistro> _canistros;
        private DiBox _diBox = DiBox.MainBox;
        
        public void Generate()
        {
            if(_canistros!=null)
                throw new Exception("Canistro alredy exist");
            Created?.Invoke(_canistros = Create());
        }

        private List<Canistro> Create()
        {
            List<Canistro> result = new List<Canistro>();
            foreach (var point in _pointForCanistor)
            {
                var newCanistro = _diBox.CreatePrefab(_canistroTemplate);
                newCanistro.transform.SetParent(point);
                result.Add(newCanistro);
                newCanistro.transform.localPosition = Vector3.zero;
                newCanistro.Init(_factoryStateCanistro);
            }
            InitStateCanistro(result);
            return result;
        }

        private void InitStateCanistro(List<Canistro> result)
        {
            List<Canistro> _canistros = new List<Canistro>();
            result.ForEach(x=>_canistros.Add(x));
            for (int i = 0; i < 4; i++)
            {
                var canistro = _canistros[Random.Range(0,_canistros.Count)];
                canistro.ChangeState<FuelState>();
                _canistros.Remove(canistro);
            }
            foreach (var c in _canistros)
                if (Random.Range(0, 1f) > 0.5f)
                    c.ChangeState<OilState>();
                else
                    c.ChangeState<EmptyState>();
        }

        private void OnDrawGizmos()
        {
            if(!_canistroTemplate)
                return;
            Gizmos.color = Color.yellow;
            _pointForCanistor.ForEach(x=>Gizmos.DrawWireCube(x.transform.position, _canistroTemplate.Size.Size));
        }
    }
}