using System;
using DG.Tweening;
using Mechanics.GameLevel.Stages.BossStagePart.Interfaces;
using Plugins.DIContainer;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Mechanics.GameLevel.Stages.BossStagePart
{
    public class Lader : MonoBehaviour, IPointerClickHandler, IPointerReciver
    {
        [SerializeField] private Transform _upPoint;
        [SerializeField] private Transform _downPoint;
        [SerializeField] private float _duration;
        [SerializeField] private Vector3 _sizeOverlap;
        [SerializeField] private Vector3 _pointOverlap;

        private Vector3 PointOverlap => transform.position + _pointOverlap;

        private Collider2D _collider;

        [DI]
        private void Init() => _collider = GetComponent<Collider2D>();

        private void MoveToPoint(Player player, Transform point, Action callbacl = null) 
            => player.transform.DOMove(point.position, _duration).OnComplete(() => callbacl?.Invoke());

        private Transform GetNearstPoint(Player player) 
            => IsUpNearToPlayerThenDown(player) ? _upPoint : _downPoint;

        private Transform GetFarPoint(Player player) 
            => IsUpNearToPlayerThenDown(player) ? _downPoint : _upPoint;

        private bool IsUpNearToPlayerThenDown(Player player) => 
            Vector3.Distance(player.transform.position, _upPoint.position) < Vector3.Distance(player.transform.position, _downPoint.position);

        public void OnPointerClick(PointerEventData eventData)
        {
            var player = FindPlayerOrNull();
            if(!player)
                return;
            player.ChangeActiveMover(false);
            player.transform.position = GetNearstPoint(player).position;
            MoveToPoint(player, GetFarPoint(player), ()=>player.ChangeActiveMover(true));
        }

        private Player FindPlayerOrNull()
        {
            foreach (var collider in Physics2D.OverlapBoxAll(PointOverlap, _sizeOverlap, 0))
                if (collider.gameObject.TryGetComponent<Player>(out var result))
                    return result;
            return null;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(PointOverlap, _sizeOverlap);
        }

        public void On() => _collider.enabled = true;

        public void Off() => _collider.enabled = false;
    }
}