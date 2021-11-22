using System;
using Mechanics.GameLevel.Stages.BossStagePart.Interfaces;
using UnityEngine;

namespace Mechanics.GameLevel.Stages.BossStagePart
{
    public class BossPart : MonoBehaviour, IBossPart
    {
        public event Action Cleaned;
        
        [SerializeField] private Sprite _dirtySprite;
        [SerializeField] private Sprite _clearSprite;
        [SerializeField] private SpriteRenderer _spriteRenderer;

        public Transform Transform => transform;
        public bool IsClear() => _spriteRenderer.sprite == _clearSprite;

        public void MakeClear()
        {
            _spriteRenderer.sprite = _clearSprite;
            Cleaned?.Invoke();
        }

        public void MakeDirty() => _spriteRenderer.sprite = _dirtySprite;
    }
}