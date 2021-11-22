using System;
using DG.Tweening;
using Factories;
using Mechanics.Interfaces;
using Plugins.DIContainer;
using UnityEngine;

namespace Mechanics.TransitPlayer
{
    public class MovePlayerToPoint : AbsTransitPlayer
    {
        [SerializeField] private Transform _playerPoint;
        [SerializeField] private float _duration;

        [DI] private FactoryPrompter _factoryPrompter;
        
        private void Awake()
        {
            if(!_playerPoint) Debug.LogError("Null point", this);
        }
        
        public override void Transit(Player player,Vector3 pointCamera,  Action callback) 
            => _factoryPrompter.Current.Say("", ()=>_factoryPrompter.Current.Hide(()=>Move(player, pointCamera, callback)));

        private void Move(Player player, Vector3 pointCamera, Action callback)
        {
            player.MoveToPoint(_playerPoint, _duration, callback);
            Camera.main.transform.DOMove(pointCamera, _duration);
        }
    }
}