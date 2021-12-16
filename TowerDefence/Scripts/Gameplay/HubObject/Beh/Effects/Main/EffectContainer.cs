using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using Plugins.HabObject;
using TMPro;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Gameplay.HubObject.Beh.Effects
{
    [BehaviourButton("Effects/Main/Container Effect")]
    public class EffectContainer : MonoBehaviour
    {
        public event Action EffectUpdate;

        [SerializeField] private HabObject _habObject;
        [RequireInterface(typeof(IEffect))][SerializeField] private List<Object> _objectEffects;
        
        private Dictionary<object, IEffect> _inGameEffects = new Dictionary<object, IEffect>();
        private List<IEffect> _objectEffectsInInterfase;

        private List<IEffect> _unsortedFullList => _inGameEffects.Values.ToList().Union(_objectEffectsInInterfase).ToList();
        private List<IEffect> _fullList;

        public event Action<IEffect> NewEffectAdd;
        public event Action<IEffect> EffectDelete;

        private void Start()
        {
            _objectEffectsInInterfase = new List<IEffect>();
            _objectEffects.ForEach(x=>
            {
                if (x != null)
                {
                    _objectEffectsInInterfase.Add((IEffect) x);
                    (x as IAddEffect)?.OnAdd(_habObject);
                }
            });
            UpdateFullList();
        }

        public void Add(IEffect effect, object id)
        {
            if(_inGameEffects.ContainsKey(id))
                return;
            var e = effect.Clone();
            _inGameEffects.Add(id, e);
            (_inGameEffects[id] as IAddEffect)?.OnAdd(_habObject);
            NewEffectAdd?.Invoke(e);
            UpdateFullList();
        }

        public void Remove(object id)
        {
            if (_inGameEffects.TryGetValue(id, out var v))
            {
                (v as IRemovedEffet)?.OnRemove(_habObject);
                EffectDelete?.Invoke(v);
                _inGameEffects.Remove(id);
                UpdateFullList();
            }
        }

        public IModificateData MakeEffect(IModificateData modificateData)
        {
            _fullList.ForEach(x=>modificateData = x.Make(_habObject, modificateData));
            return modificateData;
        }

        private void UpdateFullList()
        {
            _fullList = _unsortedFullList.OrderBy(x => x.Priority).ToList();
            EffectUpdate?.Invoke();
        }
    }
}