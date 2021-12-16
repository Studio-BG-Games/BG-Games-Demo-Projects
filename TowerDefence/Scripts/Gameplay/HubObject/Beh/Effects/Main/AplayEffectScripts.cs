using System;
using System.Collections;
using BehTreeBrick.Action;
using Gameplay.HubObject.Beh.Attributes;
using Gameplay.HubObject.Beh.Effects;
using Plugins.HabObject;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Gameplay.HubObject.Beh.Scripts
{
    [BehaviourButton("Stat/OnDeadMoment/Aplay effect on dead")]
    public class AplayEffectScripts : MonoBehaviour
    {
        [SerializeField] private SearcherTargetInRadius _searcherTargetInRadius;
        [RequireInterface(typeof(IEffect))] [SerializeField] private Object[] _effectsObj;
        [Min(0)][SerializeField] private float _at;
        [SerializeField] private Health _health;
        
        private IEffect[] _effects;

        public event Action<IEffect, HabObject> AddEffectTo;
        public event Action<HabObject> SomeEffectAddTo;

        private void Start()
        {
            _effects = new IEffect[_effectsObj.Length];
            for (var i = 0; i < _effects.Length; i++) _effects[i] = (IEffect) _effectsObj[i];
            _health.Dead += OnDead;
        }

        private void OnDead(HabObject obj) => ApplayEffect();

        private void ApplayEffect()
        {
            _searcherTargetInRadius.
                All().
                ForEach(x=>
                {
                    if (x.ComponentShell.TryGet<EffectContainer>(out var r))
                    {
                        for (int i = 0; i < _effects.Length; i++)
                        {
                            r.Add(_effects[i], GetID(i));
                            AddEffectTo?.Invoke(_effects[i], x);
                            r.StartCoroutine(TimeDestroyEffect(GetID(i), x));
                        }
                        if(_effects.Length>0)
                            SomeEffectAddTo?.Invoke(x);
                    }
                });
        }

        private string GetID(int i) => GetInstanceID().ToString() + "=UnicID=" + i.ToString();
        
        
        private IEnumerator TimeDestroyEffect(object id, HabObject habObject)
        {
            yield return new WaitForSeconds(_at);
            RemoveEffect(id, habObject);
        }
        
        private void RemoveEffect(object id, HabObject habObject)
        {
            if(!habObject)
                return;
            habObject.ComponentShell.Get<EffectContainer>().Remove(id);
        }
    }
}