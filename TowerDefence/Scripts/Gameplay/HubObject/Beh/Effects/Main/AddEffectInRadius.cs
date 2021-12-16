using System;
using System.Collections.Generic;
using BehTreeBrick.Action;
using Gameplay.Builds;
using Gameplay.HubObject.Beh.Effects.Realise.UpMaxHP;
using Gameplay.HubObject.Data;
using Gameplay.Units;
using Plugins.HabObject;
using Plugins.PhysicShell;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Gameplay.HubObject.Beh.Effects
{
    [BehaviourButton("Effects/Main/Add Effect in radius")]
    public class AddEffectInRadius : MonoBehaviour
    {
        [RequireInterface(typeof(IEffect))][SerializeField] private Object[] _effects;
        [SerializeField] private SearcherTargetInRadius _searcherTargetInRadius;

        private IEffect[] _effectsI;

        public event Action<IEffect, HabObject> AddEffectTo;
        public event Action<HabObject> AddSomeEffectTo;
        public event Action<IEffect, HabObject> DeleteEffectTo;
        public event Action<HabObject> DeleteSomeEffectTO;

        private void Awake()
        {
            _searcherTargetInRadius.NewObject += OnNewObject;
            _searcherTargetInRadius.ObjectDelete += OnDelete;
            _effectsI = new IEffect[_effects.Length];
            for (int i = 0; i < _effects.Length; i++)
            {
                _effectsI[i] = (IEffect)_effects[i];
            }
        }

        private void OnNewObject(HabObject obj)
        {
            if(TryGetEffectContainer(obj, out var container))
            {
                for (int i = 0; i < _effectsI.Length; i++)
                {
                    container.Add(_effectsI[i], GetIdByIndex(i));
                    AddEffectTo?.Invoke(_effectsI[i], obj);
                }
                if(_effectsI.Length>0)
                    AddSomeEffectTo?.Invoke(obj);
            }
        }

        private void OnDelete(HabObject obj)
        {
            if(TryGetEffectContainer(obj, out var container))
            {
                for (int i = 0; i < _effectsI.Length; i++)
                {
                    container.Add(_effectsI[i], GetIdByIndex(i));
                    DeleteEffectTo?.Invoke(_effectsI[i], obj);
                }
                if(_effectsI.Length>0)
                    DeleteSomeEffectTO?.Invoke(obj);
            }
        }

        private bool TryGetEffectContainer(HabObject habObject, out EffectContainer result)
        {
            var boolR = habObject.ComponentShell.TryGet<EffectContainer>(out var r);
            result = r;
            return boolR;
        }

        private string GetIdByIndex(int i) => GetInstanceID().ToString() + $"Effect Number {i}";
    }
}