using System;
using System.Collections.Generic;
using Gameplay.HubObject.Beh.Effects;
using Plugins.HabObject;

namespace Gameplay.HubObject.Beh.Damages
{
    public class Damage : IModificateData
    {
        public HabObject HabObject { get; }
        private readonly List<ElementDagame> _damagesElement;

        public Damage(HabObject habObject, List<ElementDagame> damagesElement)
        {
            HabObject = habObject;
            _damagesElement = damagesElement;
        }

        public void GoOverElement(Action<ElementDagame> callback) => _damagesElement.ForEach(x=>callback?.Invoke(x));

        public Damage CloneWithReplaceElement(List<ElementDagame> elementDagames) => new Damage(HabObject, elementDagames);
    }
}