using System;
using UnityEngine;

namespace Mechanics
{
    public class SizeElement : MonoBehaviour
    {
        public Vector2 Size => new Vector2(_sizeX, _sizeY);
        
        [Min(1)] [SerializeField] private float _sizeX;
        [Min(1)] [SerializeField] private float _sizeY;
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireCube(transform.position, new Vector3(_sizeX, _sizeY, 1));
        }
    }
}