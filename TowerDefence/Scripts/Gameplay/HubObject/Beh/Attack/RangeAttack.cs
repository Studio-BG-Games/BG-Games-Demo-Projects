using System;
using System.Collections.Generic;
using BehTreeBrick.Action;
using Plugins.HabObject;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Gameplay.HubObject.Beh.Attack
{
    [BehaviourButton("Combat/Attack/Range attack")]
    public class RangeAttack : AbsAttack
    {
        public override bool InProgress => _animatorApplay.IsCurrentAnimation(_nameAnimationAttack, 0);

        [SerializeField] private AnimatorApplayCallback _animatorApplay;
        [SerializeField] private string _nameAnimationAttack;
        [SerializeField] private string _nameCallbackFireMoment;
        [SerializeField] private AbsBullet bulletOneTargetTemplate;
        [SerializeField] private GiveTarget _searchTarget;
        [SerializeField] private AttackContainerBeh _attackContainerBeh;
        
        private HabObject _target;
        
        [Header("Настройки полета")]
        [SerializeField] private AnimationCurve _cureveFly = CreateDefaultCurve();
        [SerializeField] private Transform _firePoint;
        [Min(2)][SerializeField] private float _h;

        public override void Attack()
        {
            if(InProgress)
                return;
            _target = _searchTarget.GetOne();
            if(!_target)
                return;
            CallStartEvet();
            _animatorApplay.StartAnimation(_nameAnimationAttack);
            _animatorApplay.CallbackAnimation += OnHint;
        }

        private void OnHint(string obj)
        {
            if(obj!=_nameCallbackFireMoment)
                return;
            CallHintEvent(); 
            _animatorApplay.CallbackAnimation -= OnHint;
            var newBullet = Instantiate(bulletOneTargetTemplate);
            newBullet.Init(_attackContainerBeh.GetDamage(),  _target);
            newBullet.StartFly(_firePoint.position, _cureveFly, _h);
            _target = null;
        }

        private void OnValidate()
        {
            if (_cureveFly.keys.Length < 3)
                _cureveFly = CreateDefaultCurve();
            _cureveFly.keys[_cureveFly.keys.Length - 1].value = 0;
            _cureveFly.keys[0].value = 0;
        }

        private static AnimationCurve CreateDefaultCurve()
        {
            var k1=new Keyframe(0,0);
            var k2=new Keyframe(0.5f,1);
            var k3=new Keyframe(1,0);
            AnimationCurve result = new AnimationCurve(new []{k1,k2,k3});
            return result;
        }
    }
}