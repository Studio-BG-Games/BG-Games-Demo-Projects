using System;
using System.Collections;
using Gameplay.StateMachine.GameScene;
using Plugins.DIContainer;
using UnityEngine;

namespace Gameplay.Units.Beh
{
    [BehaviourButton("Combat/Zone move")]
    public class ZoneMove : MonoBehaviour
    {
        private Vector3 _center;
        private Vector3 _rotate;

        public Vector3 Rotate => _rotate;
        [SerializeField][Min(0.2f)]private float _radius=2;
        [SerializeField][Min(0.1f)] private float _centerZone=1;
        [SerializeField] private Unit _unit;


        public bool InZone => Vector3.Distance(_center, _unit.transform.position) < _radius;
        public bool InCenter =>Vector3.Distance(_center, _unit.transform.position) < _centerZone;
        public Vector3 Center => _center;
        
        private void Awake()
        {
            UpdateCenter();
        }

        [ContextMenu("Update Center")]
        public void UpdateCenter()
        {
            _rotate = transform.eulerAngles;
            _center = transform.position;
        }

        [ContextMenu("DebugZone")]
        private void DebugZones()
        {
            Debug.Log("INZone = "+InZone);
            Debug.Log("INCenter = "+InCenter);
        }


        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(_center, _radius);
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(_center, _centerZone);
        }

        private void OnValidate()
        {
            UpdateCenter();
            if (_centerZone >= _radius)
                _centerZone = _radius - 0.5f;
        }
    }
}