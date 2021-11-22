using System;
using Factories;
using Mechanics.Interfaces;
using Plugins.DIContainer;
using Plugins.Interfaces;
using UnityEngine;

namespace Mechanics.TransitPlayer
{
    public class TeleportPlayerTransit : AbsTransitPlayer
    {
        [SerializeField] private Transform _point;
        [SerializeField] private float _duration;
        
        [DI] private Curtain _curtain;
        [DI] private FactoryPrompter _factoryPrompter;

        private void Awake()
        {
            if(!_point) Debug.LogError("Null point", this);
        }

        public override void Transit(Player player,Vector3 pointCamera,  Action callback)
            => _factoryPrompter.Current.Say("", 
                ()=>_factoryPrompter.Current.Hide(
                    ()=>Move(player, pointCamera, callback)
                    )
                );

        private void Move(Player player, Vector3 pointCamera, Action callback)
        {
            _curtain.Fade(() =>
            {
                player.transform.position = _point.position;
                callback?.Invoke();
                _curtain.Unfade();
                Camera.main.transform.position = pointCamera;
            }, _duration);
        }
    }
}