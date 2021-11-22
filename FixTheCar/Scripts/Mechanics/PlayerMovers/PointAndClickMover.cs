using System;
using System.Collections;
using Plugins.DIContainer;
using Services.IInputs;
using Services.Interfaces;
using UnityEngine;

namespace Mechanics
{
    public class PointAndClickMover : PlayerMover
    {
        [Header("Настройки клика")] 
        [SerializeField] private float _minDistanceToMove;
        [SerializeField] private float _distanceToTargetForStop;
        [SerializeField] private float _lenghtRayCastForStop;
        [SerializeField] private Vector3 _offsetForStartPointRay;
        [SerializeField] private LayerMask _mask;

        private Vector3 StartPointForRay => transform.position + _offsetForStartPointRay;
        private DiractionMove _diractionMove;
        private Coroutine _moveAction;
        
        [DI] private IInput _input;
        private bool _isStop;

        [DI]
        private void Init() => Off();

        public override void SetFakeStop(bool isStop) => _isStop = isStop;
        
        public override void On()
        {
            base.On();
            _input.RayCastInGameField += OnClickOnScreen;
            _isStop = false;
        }

        public override void Off()
        {
            base.Off();
            _input.RayCastInGameField -= OnClickOnScreen;
        }

        private void OnClickOnScreen(Vector2 obj)
        {
            if (Mathf.Abs(transform.position.x - obj.x) > _minDistanceToMove)
            {
                if (_moveAction!=null)
                    StopCoroutine(_moveAction);
                _moveAction = StartCoroutine(Move(obj));
            }
        }

        private IEnumerator Move(Vector3 pointToMove)
        {
            _diractionMove = GetDiractionMoveByPoint(pointToMove);
            Player.SetMoveAnimationActive(true);
            SwitchSpriteByDiraction(_diractionMove);
            while (CanMove(pointToMove))
            {
                Rigidbody2D.velocity = GetVeocityByDiraction(_diractionMove);
                yield return null;
            }
            Player.SetMoveAnimationActive(false);
            Rigidbody2D.velocity = Vector2.zero;
        }

        private Vector2 GetVeocityByDiraction(DiractionMove diractionMove) => GetDiractionVectorByDiractionEnum() * Speed;

        private bool CanMove(Vector3 target)
        {
            if (Math.Abs(target.x - transform.position.x) < _distanceToTargetForStop || !enabled || _isStop)
                return false;
            RaycastHit2D[] hit = Physics2D.RaycastAll(StartPointForRay, GetDiractionVectorByDiractionEnum(), _lenghtRayCastForStop, _mask.value);
            return hit.Length <= 1;
        }

        private Vector2 GetDiractionVectorByDiractionEnum() => _diractionMove == DiractionMove.ToRight ? Vector2.right: Vector2.left;

        private void SwitchSpriteByDiraction(DiractionMove diractionMove) => SpriteRenderer.flipX = diractionMove == DiractionMove.ToLeft;

        private DiractionMove GetDiractionMoveByPoint(Vector3 pointToMove) => pointToMove.x > transform.position.x ? DiractionMove.ToRight : DiractionMove.ToLeft;

        private enum DiractionMove
        {
            ToRight, ToLeft
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.magenta;
            Vector3 leftBorderClick = transform.position;
            leftBorderClick.x -= _minDistanceToMove;
            Vector3 rightBorderClick = transform.position;
            rightBorderClick.x += _minDistanceToMove;
            Gizmos.DrawLine(leftBorderClick, rightBorderClick);
            
            Gizmos.color = Color.yellow;
            Vector3 rightPointRay = StartPointForRay;
            rightPointRay.x -= _lenghtRayCastForStop;
            Vector3 leftPointRay = StartPointForRay;
            leftPointRay.x += _lenghtRayCastForStop;
            Gizmos.DrawLine(rightPointRay, leftPointRay);
            
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(transform.position, _distanceToTargetForStop);
        }
    }
}