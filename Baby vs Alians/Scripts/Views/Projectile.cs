using UnityEngine;

namespace Baby_vs_Aliens
{
    public class Projectile : MonoBehaviour, IProjectile
    {
        #region Fields

        private Vector3 _moveDirection;
        private ProjectileConfig _config;
        private float _lifeTime;
        private bool _isActive;

        private Rigidbody _rigidBody;

        #endregion


        #region Properties

        public EntityType Type => _config.ProjectileOwner;

        public int Damage => _config.Damage;

        #endregion


        #region IProjectile

        public void Init(Vector3 startingPosition, Vector3 moveDirection, ProjectileConfig config)
        {
            _config = config;
            transform.position = startingPosition;
            _moveDirection = moveDirection;
            _lifeTime = _config.LifeTime;
            _isActive = true;

            _rigidBody.velocity = _moveDirection * _config.Velocity;
        }

        private void DisableProjectile()
        {
            _isActive = false;
            _rigidBody.velocity = Vector3.zero;
            ServiceLocator.GetService<ObjectPoolManager>().GetBulletPool(_config.Prefab).Push(this.gameObject);
        }

        #endregion


        #region UnityMethods

        private void Awake()
        {
            _rigidBody = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            if (!_isActive)
                return;

            if (_lifeTime > 0)
                _lifeTime -= Time.deltaTime;
            else
                DisableProjectile();
        }

        private void OnTriggerEnter(Collider other)
        {
            var damageable = other.GetComponent<IDamageable>();

            if (damageable != null && damageable.Type != Type)
                damageable.TakeDamege(Damage);

            DisableProjectile();
        }

        #endregion
    }
}