using System;
using UnityEngine;
using UnityEngine.AI;

namespace Baby_vs_Aliens
{
    public class EnemyView : MonoBehaviour, IDamageable
    {
        #region Fields

        [SerializeField] EntityType _type = EntityType.Alien;
        [SerializeField] Transform _bulletSpawn;
        [SerializeField] EnemyConfig _config;
        private NavMeshAgent _navMeshAgent;
        private Animator _animator;

        private Vector3 _previousDestination;

        public event Action<int> Damaged;

        private const string RESET_TRIGGER = "Reset";
        private const string DEATH_TRIGGER = "Death";
        private const string ATTACK_TRIGGER = "Attack";
        private const string DAMAGED_TRIGGER = "Hurt";
        private const string SPEED_FLOAT = "Speed";

        #endregion


        #region Properties

        public EntityType Type => _type;

        public Vector3 BulletSpawn => _bulletSpawn.position;

        public EnemyConfig Config => _config;

        public bool HasReachedDestination => _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance;

        #endregion


        #region UnityMethods

        private void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _animator = GetComponentInChildren<Animator>();
        }

        void Update()
        {
            _animator.SetFloat(SPEED_FLOAT, _navMeshAgent.velocity.magnitude);
        }

        #endregion


        #region Methods

        public void Init()
        {
            _animator.SetTrigger(RESET_TRIGGER);
        }

        public void InitAtPosition(Vector3 position)
        {
            position.y = transform.position.y;
             _navMeshAgent.Warp(position);
        }

        public void SetDestination(Vector3 newDestination)
        {
            newDestination.y = transform.position.y;
            _navMeshAgent.destination = newDestination;
        }

        public void Attack()
        {
            _animator.SetTrigger(ATTACK_TRIGGER);
        }

        public void OnDamaged()
        {
            _animator.SetTrigger(DAMAGED_TRIGGER);
        }

        public void ResumePath()
        {
            SetDestination(_previousDestination);
        }

        public void Stop()
        {
            _previousDestination = _navMeshAgent.destination;
            _navMeshAgent.ResetPath();
        }

        public void Die()
        {
            Stop();
            _animator.SetTrigger(DEATH_TRIGGER);
        }

        public void TakeDamege(int damage)
        {
            Damaged?.Invoke(damage);
        }

        #endregion
    }
}