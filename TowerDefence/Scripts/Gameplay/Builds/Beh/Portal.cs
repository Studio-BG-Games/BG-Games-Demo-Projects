using System;
using System.Collections.Generic;
using System.Linq;
using Factorys;
using Gameplay.Map;
using Gameplay.Units;
using Plugins.DIContainer;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Gameplay.Builds.Beh
{
    [BehaviourButton("Other/Portal (spawn point)")]
    public class Portal : MonoBehaviour
    {
        [DI] private FactoryUnit _factoryUnit;
        [DI] private WorldShell _worldShell;

        [SerializeField] private List<Transform> _pointToSpawn = new List<Transform>();
        [Min(0)][SerializeField] private float _radiusCheclClearPoint;

        private Collider[] _results;

        public event Action<Unit> EnemyCreated;
        public event Action<Unit, Vector3> EnemyCreatedAtPoint;
        
        private int XSize => _worldShell.World.chunkSize * _worldShell.World.sectorSize * _worldShell.DataMap.MapSettings.Size.x;
        private int YSize => _worldShell.World.chunkSize * _worldShell.World.sectorSize * _worldShell.DataMap.MapSettings.Size.y;

        private void Awake()
        {
            _results = new Collider[1];
        }

        public void Init() => RemovePoints();
        
        private void RemovePoints()
        {
            List<Transform> pointTOdelete = new List<Transform>();
            foreach (var point in _pointToSpawn)
            {
                var x = point.transform.position.x;
                var z = point.transform.position.z;
                if (x > XSize || z > YSize || x < 0 || z < 0)
                    pointTOdelete.Add(point);
            }

            _pointToSpawn = _pointToSpawn.Except(pointTOdelete).ToList();
            if (_pointToSpawn.Count <= 0)
                Debug.LogWarning("Portal don't has point for spawn!!!");
        }

        public bool TrySpawn(Unit unit)
        {
            Shafel(_pointToSpawn);
            for (int i = 0; i < _pointToSpawn.Count; i++)
            {
                _results[0] = null;
                 Physics.OverlapSphereNonAlloc(_pointToSpawn[i].position, _radiusCheclClearPoint, _results, 1 << 8);
                 if (_results[0] == null)
                 {
                     var e = Spawn(unit, _pointToSpawn[i]);
                     EnemyCreated?.Invoke(e);
                     EnemyCreatedAtPoint?.Invoke(e, _pointToSpawn[i].position);
                     return true;
                 }
            }

            return false;
        }

        private Unit Spawn(Unit unit, Transform transform1) => _factoryUnit.SpawnEnemy(unit, transform1);

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            foreach (var point in _pointToSpawn)
            {
                Gizmos.DrawWireSphere(point.position, _radiusCheclClearPoint);
            }
        }

        private void Shafel(List<Transform> pointToSpawn)
        {
            for (int i = 0; i < pointToSpawn.Count; i++)
            {
                pointToSpawn[i] = pointToSpawn[Random.Range(0, pointToSpawn.Count)];
            }
        }
    }
}