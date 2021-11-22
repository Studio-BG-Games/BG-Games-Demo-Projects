using System;
using System.Collections;
using Mechanics.GameLevel.Stages.BossStagePart.Interfaces;
using UnityEngine;

namespace Mechanics.GameLevel.Stages.BossStagePart
{
    [RequireComponent(typeof(Collider2D))]
    public class klash : MonoBehaviour, IBossPart, IInitPlayerHealth
    {
        public event Action Cleaned;
        
        [SerializeField] private Stick _stick;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Sprite _clearSprite;
        [SerializeField] private Sprite _dirtySprite;
        [SerializeField] private float _blockAttackTime = 2;

        private bool _canAttack = true;
        private PlayerHelth _playerHelth;

        private void Update() => transform.position = _stick.PointKlash.position;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<Player>(out var player) && _canAttack)
            {
                _playerHelth.Damage();
                StartCoroutine(AttackBlockTimer(_blockAttackTime));
            }
        }

        #region IBOSSPART

        public Transform Transform => transform;
        public bool IsClear() => _spriteRenderer.sprite == _clearSprite;

        public void MakeDirty() => _spriteRenderer.sprite = _dirtySprite;

        public void MakeClear()
        {
            _spriteRenderer.sprite = _clearSprite;
            Cleaned?.Invoke();
        }

        #endregion

        public void Init(PlayerHelth health) => _playerHelth = health;

        private IEnumerator AttackBlockTimer(float time)
        {
            _canAttack = false;
            yield return new WaitForSeconds(time);
            _canAttack = true;
        }
    }
}