using UnityEngine;

namespace Baby_vs_Aliens
{
    [CreateAssetMenu(fileName = "PlayerConfig", menuName = "Data/PlayerConfig")]
    public class PlayerConfig : ScriptableObject
    {
        [SerializeField, Min(0.1f)] private float _speed;
        [SerializeField, Min(1)] private int _maxLives;
        [SerializeField, Min(1)] private int _maxHealth;
        [SerializeField, Min(0)] private float _damageRecoveryTime;
        [SerializeField, Min(0)] private float _respawnTime;
        [SerializeField] private ProjectileConfig _projectileConfig;
        [SerializeField] private PlayerView _playerPrefab;

        public float Speed => _speed;
        public int MaxLives => _maxLives;
        public int MaxHealth => _maxHealth;
        public float DamageRecoveryTime => _damageRecoveryTime;
        public float RespawnTime => _respawnTime;
        public ProjectileConfig ProjectileConfig => _projectileConfig;
        public PlayerView PlayerPrefab => _playerPrefab;
    }
}