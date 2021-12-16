using System;
using System.Collections;
using System.Collections.Generic;
using Gameplay.HubObject.Beh.Attributes;
using Gameplay.HubObject.Beh.Damages;
using Plugins.HabObject;
using UnityEngine;

namespace Gameplay.HubObject.Beh.Attack
{
    public abstract class AbsBullet : MonoBehaviour
    {
        [Min(0)][SerializeField] private float _speed;
        [Min(3)][SerializeField] private int _steps;
        
        protected Damage Damage;
        protected HabObject Target;

        private Vector3 _from;
        private Vector3 _to;
        private AnimationCurve _curveFly;
        private float _maxH;
        private Coroutine _flyAction;
        
        private void Awake() => gameObject.SetActive(false);

        public void Init(Damage damage, HabObject targetMove)
        {
            Damage = damage;
            Target = targetMove;
        }
        
        public void StartFly(Vector3 from, AnimationCurve curveFly, float H)
        {
            if(!Target)
                Destroy(gameObject);
            if(_flyAction!=null)
                return;
            gameObject.SetActive(true);
            _from = from;
            transform.position = from;
            _curveFly = curveFly;
            _maxH = H;
            Target.Destroyed += OnDestroyTarget;
            _flyAction = StartCoroutine(Move());
        }

        void OnDestroyTarget(HabObject obj)
        {
            Destroy(gameObject);
            if(_flyAction!=null)
                StopCoroutine(_flyAction);
        }
        
        private void OnDestroy()
        {
            if(Target)
                Target.Destroyed -= OnDestroyTarget;
        }

        private IEnumerator Move()
        {
            for (int i = 0; i < _steps; i++)
            {
                if(Target)
                    _to = Target.transform.position;
                yield return MoveToPoint(i);
            }
            Destroy(gameObject);
            var target = GetTargets();
            if (target != null)
            {
                for (var i = 0; i < target.Count; i++)
                {
                    if(i>=target.Count)
                        break;
                    if(target[i]==null)
                        continue;
                    target[i].ComponentShell.Get<Health>().Damage(Damage);
                }
            }
        }

        protected abstract List<HabObject> GetTargets();

        private IEnumerator MoveToPoint(int step)
        {
            Vector3 point = GetPoint(step);
            while (transform.position != point)
            {
                transform.position = Vector3.MoveTowards(transform.position, point, _speed * Time.deltaTime);
                transform.rotation = Quaternion.Slerp(transform.rotation, GetLookAtPoint(point), _speed * Time.deltaTime);
                yield return null;
            }
        }

        private Quaternion GetLookAtPoint(Vector3 point)
        {
            var tempRotate = transform.rotation;
            transform.LookAt(point);
            var result = transform.rotation;
            transform.rotation = tempRotate;
            return result;
        }

        private Vector3 GetPoint(int step)
        {
            step = Mathf.Clamp(step, 0, _steps);
            float key = (float) step / _steps;
            Vector3 result = _from * (1 - key) + _to * key;
            result.y += _maxH * _curveFly.Evaluate(key);
            return result;
        }
    }
}