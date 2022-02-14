using UnityEngine;
using System;

namespace Baby_vs_Aliens
{
    public class PlayerView : MonoBehaviour, IDamageable
    {
        #region Fields

        [SerializeField] private EntityType _type = EntityType.Baby;
        [SerializeField] private ParticleSystem _bubblesShot;
        [SerializeField] private ParticleSystem _bubblesDeath;
        [SerializeField] private GameObject _characterModel;

        [SerializeField] private HealthBar _healthBar;

        private Transform _gunBone;
        private SkinnedMeshRenderer _meshRenderer;

        private Animator _animator;
        private Rigidbody _rigidBody;

        private Vector3 _moveVector;

        private CharacterState _currentState;

        private CapsuleCollider _collider;

        public event Action<int> Damaged;

        private const int SHIRT_MATERIAL_INDEX = 5;
        private const int HAIR_MATERIAL_INDEX = 9;

        private bool _isInitiated;

        #endregion


        #region Properties

        public Animator Animator => _animator;

        public Rigidbody RigidBody => _rigidBody;

        public EntityType Type => _type;

        public Vector3 BulletSpawn => transform.position + _collider.center + transform.forward * _collider.radius * 2;

        public ParticleSystem ShotParticles => _bubblesShot;

        public ParticleSystem DeathParticles => _bubblesDeath;

        public HealthBar HealthBar => _healthBar;

        #endregion


        #region UnityMethods

        private void FixedUpdate()
        {
            if (!_isInitiated)
                return;

            var oldPosition = _rigidBody.position;
            var newPosition = oldPosition + _moveVector;

            _rigidBody.MovePosition(newPosition);
            _moveVector = Vector3.zero;
        }

        #endregion


        #region Methods

        public void SetMoveVector(Vector3 vector)
        {
            _moveVector = vector;
        }

        public void SetState(CharacterState state)
        {
            if (_currentState != state)
            {
                _currentState = state;

                _animator.SetTrigger(state.ToString());
            }
        }

        public void TakeDamege(int damage)
        {
            Damaged?.Invoke(damage);
        }

        public void HideCharacter()
        {
            _characterModel.SetActive(false);
            _collider.enabled = false;
            _healthBar.gameObject.SetActive(false);
        }

        public void ShowCharacter()
        {
            SetState(CharacterState.Idle);
            _characterModel.SetActive(true);
            _collider.enabled = true;
            _healthBar.gameObject.SetActive(true);
        }

        public void InitCharacterModel(GameObject characterModel)
        {
            characterModel.transform.parent = _characterModel.transform;
            characterModel.transform.localPosition = Vector3.zero;
            characterModel.transform.localRotation = Quaternion.identity;

            _animator = GetComponentInChildren<Animator>();
            _rigidBody = GetComponent<Rigidbody>();
            _collider = GetComponent<CapsuleCollider>();

            SetState(CharacterState.Idle);

            _isInitiated = true;
        }

        public void InitCustomLooks(GameObject gun, Material shirtMaterial, Material hairMaterial)
        {
            var children = GetComponentsInChildren<Transform>();

            if (_gunBone == null)
            {
                foreach (var child in children)
                    if (child.CompareTag("GunHolder"))
                    {
                        _gunBone = child;
                        break;
                    }
            }

            if (_gunBone != null)
            {
                foreach (var child in children)
                    if (child.CompareTag("DefaultGun"))
                    {
                        child.gameObject.SetActive(false);
                        break;
                    }

                gun.transform.parent = _gunBone;
                gun.transform.localPosition = Vector3.zero;
                gun.transform.localRotation = Quaternion.identity;
            }

            if (_meshRenderer == null)
                _meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();

            var materials = _meshRenderer.materials;

            materials[SHIRT_MATERIAL_INDEX] = shirtMaterial;
            materials[HAIR_MATERIAL_INDEX] = hairMaterial;

            _meshRenderer.materials = materials;
        }

        #endregion
    }
}