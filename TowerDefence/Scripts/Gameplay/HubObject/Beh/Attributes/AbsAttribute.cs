using System;
using Gameplay.HubObject.Beh.Effects;
using UnityEngine;

namespace Gameplay.HubObject.Beh.Attributes
{
    public abstract class AbsAttribute : MonoBehaviour
    {
        [Header("Can be empty")]
        [SerializeField] protected EffectContainer EffectContainer;
        
        private void Awake()
        {
            if (EffectContainer)
                EffectContainer.EffectUpdate += UpdateByEffect;
            else
                OnUpdateBuff(GetCurrent());
            CustomAwake();
        }

        public void UpdateByEffect()
        {
            if(EffectContainer) OnUpdateBuff(EffectContainer.MakeEffect(GetCurrent()));
            else OnUpdateBuff(GetCurrent());
        }

        public abstract IModificateData GetCurrent();
        protected abstract void OnUpdateBuff(IModificateData modificateData);

        protected virtual void CustomAwake(){}
    }
}