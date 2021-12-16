using BehTreeBrick.Action;
using Plugins.HabObject;
using UnityEngine;

namespace Gameplay.HubObject.Beh.GiverTarget.PartSearchInTarget
{
    [System.Serializable]
    public class ContainerPartSearch : IPartSearchRadius
    {
        [RequireInterface(typeof(IPartSearchRadius))][SerializeField] private Object[] _parts;
        private IPartSearchRadius[] _IParts;

        public SearcherTargetInRadius.ResultCheck IsCorrect(HabObject habObject)
        {
            foreach (var part in _IParts)
            {
                if (part.IsCorrect(habObject) == SearcherTargetInRadius.ResultCheck.ToDelete)
                    return SearcherTargetInRadius.ResultCheck.ToDelete;
            }

            return SearcherTargetInRadius.ResultCheck.Correct;
        }

        public IPartSearchRadius Clone(HabObject parrent)
        {
            var result = new ContainerPartSearch();
            result._IParts = new IPartSearchRadius[_parts.Length];
            result._parts = _parts;
            for (var i = 0; i < result._IParts.Length; i++)
            {
                result._IParts[i] = Cast(_parts[i]).Clone(parrent);
            }
            return result;
        }

        private IPartSearchRadius Cast(Object obj) => (IPartSearchRadius) obj;
    }
}