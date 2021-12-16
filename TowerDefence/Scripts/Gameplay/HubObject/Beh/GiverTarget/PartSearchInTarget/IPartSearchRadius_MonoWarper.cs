using BehTreeBrick.Action;
using Plugins.HabObject;
using UnityEngine;

namespace Gameplay.HubObject.Beh.GiverTarget.PartSearchInTarget
{
    public abstract class IPartSearchRadius_MonoWarper : MonoBehaviour, IPartSearchRadius
    {
        public SearcherTargetInRadius.ResultCheck IsCorrect(HabObject habObject) => GetPart().IsCorrect(habObject);
        public IPartSearchRadius Clone(HabObject parrent) => GetPart().Clone(parrent);

        protected abstract IPartSearchRadius GetPart();
    }
}