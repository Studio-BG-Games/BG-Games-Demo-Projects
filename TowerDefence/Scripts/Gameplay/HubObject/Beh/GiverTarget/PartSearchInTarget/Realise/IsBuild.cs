using BehTreeBrick.Action;
using Gameplay.Builds;
using Plugins.HabObject;

namespace Gameplay.HubObject.Beh.GiverTarget.PartSearchInTarget
{
    [System.Serializable]
    public class IsBuild: IPartSearchRadius
    {
        public SearcherTargetInRadius.ResultCheck IsCorrect(HabObject target)
        {
            if (target is Build)
                return SearcherTargetInRadius.ResultCheck.Correct;
            return SearcherTargetInRadius.ResultCheck.ToDelete;
        }

        public IPartSearchRadius Clone(HabObject parrent) => this;
    }
}