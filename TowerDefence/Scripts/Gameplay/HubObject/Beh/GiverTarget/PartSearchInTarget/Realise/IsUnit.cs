using BehTreeBrick.Action;
using Gameplay.Builds;
using Gameplay.Units;
using Plugins.HabObject;

namespace Gameplay.HubObject.Beh.GiverTarget.PartSearchInTarget
{
    [System.Serializable]
    public class IsUnit: IPartSearchRadius
    {
        public SearcherTargetInRadius.ResultCheck IsCorrect(HabObject target)
        {
            if (target is Unit)
                return SearcherTargetInRadius.ResultCheck.Correct;
            return SearcherTargetInRadius.ResultCheck.ToDelete;
        }

        public IPartSearchRadius Clone(HabObject parrent) => this;
    }
}