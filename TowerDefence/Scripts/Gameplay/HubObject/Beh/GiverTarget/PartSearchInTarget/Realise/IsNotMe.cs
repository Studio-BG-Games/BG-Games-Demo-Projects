using BehTreeBrick.Action;
using Plugins.HabObject;

namespace Gameplay.HubObject.Beh.GiverTarget.PartSearchInTarget
{
    [System.Serializable]
    public class IsNotMe: IPartSearchRadius
    {
        private HabObject me;
        
        public SearcherTargetInRadius.ResultCheck IsCorrect(HabObject target)
        {
            if (target != me)
                return SearcherTargetInRadius.ResultCheck.Correct;
            return SearcherTargetInRadius.ResultCheck.ToDelete;
        }

        public IPartSearchRadius Clone(HabObject parrent)
        {
            var r =new IsNotMe();
            r.me = parrent;
            return r;
        }
    }
}