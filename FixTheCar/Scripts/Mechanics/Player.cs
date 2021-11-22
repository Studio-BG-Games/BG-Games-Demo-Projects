using System;
using DG.Tweening;
using UnityEngine;

namespace Mechanics
{
    [RequireComponent(typeof(Animator), typeof(PlayerMover))]
    public class Player : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        
        private PlayerMover _playerMover;
        
        private static readonly int IsDirty = Animator.StringToHash("IsDirty");
        private static readonly int IsMove = Animator.StringToHash("IsMove");

        private void Awake() => _playerMover = GetComponent<PlayerMover>();

        public void MoveToPoint(Transform point, float duration, Action callback= null)
        {
            transform.DOMove(point.position, duration).OnStart(()=>_animator.SetBool(IsMove, true)).OnComplete(() =>
            {
                _animator.SetBool(IsMove, false);
                callback?.Invoke();
            });
        }

        public void SetMoveAnimationActive(bool isActive) => _animator.SetBool(IsMove, isActive);

        public void MakeDirty() => _animator.SetBool(IsDirty, true);

        public void MakeClear() => _animator.SetBool(IsDirty, false);

        public void ChangeActiveMover(bool toActive)
        {
            if(toActive) _playerMover.On();
            else _playerMover.Off();
        }

        public void MakeFakeStopMove(bool isStop) => _playerMover.SetFakeStop(isStop);
    }
}