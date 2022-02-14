using UnityEngine;

namespace Baby_vs_Aliens
{
    [CreateAssetMenu(fileName = "EnemyConfig", menuName = "Data/EnemyConfig")]
    public class EnemyConfig : ScriptableObject
    {
        #region Fields

        [SerializeField] EnemyType _enemyType;
        [SerializeField, Min(1)] int _health;
        [SerializeField, Min(0)] int _scoreValue;
        [Space]
        [SerializeField] ProjectileConfig _projectileConfig;
        [Space]
        [SerializeField, Min(0)] float _minTimeBetweenAttacks;
        [SerializeField, Min(0)] float _maxTimeBetweenAttacks;
        [Space]
        [SerializeField, Min(0)] float _minTimeBeforeReroute;
        [SerializeField, Min(0)] float _maxTimeBeforeReroute;
        [SerializeField] bool _rerouteOnDestinationReached;
        [Space]
        [SerializeField] EnemyBehaviour _enemyBehaviour;
        [SerializeField, Min(0)] float _distanceToPlayer;
        [Space]
        [SerializeField, Min(0.1f)] float _attackRotationSpeed;
        [SerializeField, Min(0)] float _attackDelay;
        [SerializeField, Min(0)] float _attackRecovery;
        [SerializeField, Min(0)] float _timeToStayAfterDeath;

        #endregion


        #region Properties

        public EnemyType EnemyType => _enemyType;
        public int Health => _health;
        public int ScoreValue => _scoreValue;
        public ProjectileConfig ProjectileConfig => _projectileConfig;
        public float MinTimeBetweenAttacks => _minTimeBetweenAttacks;
        public float MaxTimeBetweenAttacks =>_maxTimeBetweenAttacks;
        public float MinTimeBeforeReroute =>_minTimeBeforeReroute;
        public float MaxTimeBeforeReroute =>_maxTimeBeforeReroute;
        public bool RerouteOnDestinationReached => _rerouteOnDestinationReached;
        public EnemyBehaviour EnemyBehaviour => _enemyBehaviour;
        public float DistanceToPlayer =>_distanceToPlayer;
        public float AttackRotationSpeed => _attackRotationSpeed;
        public float TimeToStayAfterDeath => _timeToStayAfterDeath;
        public float AttackDelay => _attackDelay;
        public float AttackRecovery => _attackRecovery;

        #endregion
    }
}