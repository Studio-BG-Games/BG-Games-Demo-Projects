using System;
using Gameplay.HubObject.Beh.Attributes;
using Gameplay.HubObject.Beh.Damages;
using Plugins.HabObject;
using UnityEngine;

namespace Gameplay
{
    [BehaviourButton("Stat/Healthf/Damage by")]
    public class DamageBy : MonoBehaviour
    {
        [SerializeField] private Health _health;

        public HabObject Target => _target;
        public bool HasTarget => _target != null;
        private HabObject _target;

        private void Awake()
        {
            _health.DamagedBy += OnDamageBy;
        }

        private void OnDamageBy(Damage obj)
        {
            if (_target)
                return;
            if (!obj.HabObject.ComponentShell.TryGet<Health>(out var health))
                return;
            if(health.Current<=0)
                return;
            health.Dead += OnDead;
            _target = obj.HabObject;
        }

        private void OnDead(HabObject obj)
        {
            _target.ComponentShell.Get<Health>().Dead -= OnDead;
            _target = null;
        }
    }
}