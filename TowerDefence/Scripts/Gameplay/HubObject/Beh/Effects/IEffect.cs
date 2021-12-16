using System.Xml.Schema;
using Plugins.HabObject;

namespace Gameplay.HubObject.Beh.Effects
{
    public interface IEffect
    {
        IModificateData Make(HabObject habObject, IModificateData modificateData);
        
        IEffect Clone();
        int Priority { get; }
    }

    public interface IAddEffect : IEffect
    {
        void OnAdd(HabObject habObject);
        
    }

    public interface IRemovedEffet : IEffect
    {
        void OnRemove(HabObject habObject);
    }
}