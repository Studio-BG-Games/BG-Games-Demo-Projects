using BehTreeBrick.Action;
using Plugins.HabObject;
using UnityEngine;

namespace Gameplay.HubObject.Beh.GiverTarget.PartSearchInTarget
{
    public abstract class IPartSearchRadius_SOWarper : ScriptableObject, IPartSearchRadius
    {
        public SearcherTargetInRadius.ResultCheck IsCorrect(HabObject habObject) => GetPart().IsCorrect(habObject);
        protected abstract IPartSearchRadius GetPart();
        public IPartSearchRadius Clone(HabObject parrent) => GetPart().Clone(parrent);
    }
}