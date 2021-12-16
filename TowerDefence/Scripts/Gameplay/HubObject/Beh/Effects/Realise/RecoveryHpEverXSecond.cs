using System.Collections;
using Gameplay.HubObject.Beh.Attributes;
using Plugins.HabObject;
using UnityEngine;

namespace Gameplay.HubObject.Beh.Effects.Realise
{
    [System.Serializable]    
    public class RecoveryHpEverXSecond : IAddEffect, IRemovedEffet
    {
        [Min(0)][SerializeField] private int _valueToAdd;
        [Min(0)] [SerializeField] private float _timeToNextAdd;
        private Coroutine _actionUpdateHealth;

        public RecoveryHpEverXSecond(int value)
        {
            if (value < 0)
                value = 0;
            _valueToAdd = value;
        }

        public IModificateData Make(HabObject habObject, IModificateData modificateData)
        {
            var data = modificateData as Health.ModificateData;
            if (data == null)
                return modificateData;
            data.Current = Mathf.Clamp(data.Current + _valueToAdd, 0, data.Max);
            return data;
        }

        public IEffect Clone() => new RecoveryHpEverXSecond(_valueToAdd);

        public int Priority => 3;
        public void OnRemove(HabObject habObject)
        {
            if(_actionUpdateHealth!=null)
                habObject.StopCoroutine(_actionUpdateHealth);
        }

        public void OnAdd(HabObject habObject)
        {
            if (habObject.ComponentShell.TryGet(out Health health)) 
                _actionUpdateHealth = habObject.StartCoroutine(Update(health));
        }

        private IEnumerator Update(Health health)
        {
            while (true)
            {
                yield return new WaitForSeconds(_timeToNextAdd);
                health.UpdateByEffect();
            }
        }
    }
}