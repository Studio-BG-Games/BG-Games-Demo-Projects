using Plugins.HabObject;
using UnityEngine;

namespace Gameplay.HubObject.Beh.Effects.Realise.UpMaxHP
{
    public abstract class EffectMonoWarper : MonoBehaviour, IEffect, IAddEffect, IRemovedEffet
    {
        protected abstract IEffect _someEffect { get; }

        public IModificateData Make(HabObject habObject, IModificateData modificateData) 
            => _someEffect.Make(habObject, modificateData);

        public IEffect Clone() => _someEffect.Clone();

        public int Priority => _someEffect.Priority;
        public void OnRemove(HabObject habObject) => (_someEffect as IRemovedEffet)?.OnRemove(habObject);

        public void OnAdd(HabObject habObject) => (_someEffect as IAddEffect)?.OnAdd(habObject);
    }
}