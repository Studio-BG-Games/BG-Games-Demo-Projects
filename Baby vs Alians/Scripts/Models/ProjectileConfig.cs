using System;
using UnityEngine;

namespace Baby_vs_Aliens
{
    [Serializable]
    public struct ProjectileConfig
    {
        [SerializeField] private EntityType _projectileOwner;
        [SerializeField] private int _damage;
        [SerializeField, Min(0.1f)] private float _velocity;
        [SerializeField] private Projectile _prefab;
        [SerializeField, Min(0)] private float _fireDelay;
        [SerializeField, Min(0.1f)] private float _lifeTime;

        public EntityType ProjectileOwner => _projectileOwner;
        public int Damage => _damage;
        public float Velocity => _velocity;
        public GameObject Prefab => _prefab.gameObject;
        public float FireDelay => _fireDelay;
        public float LifeTime => _lifeTime;
    }
}