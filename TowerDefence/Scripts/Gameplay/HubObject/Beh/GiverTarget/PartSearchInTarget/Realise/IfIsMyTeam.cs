using BehTreeBrick.Action;
using Gameplay.HubObject.Data;
using Plugins.HabObject;
using UnityEngine;

namespace Gameplay.HubObject.Beh.GiverTarget.PartSearchInTarget
{
    [System.Serializable]
    public class IfIsMyTeam : IPartSearchRadius
    {
        [SerializeField] private HabObject _me;
        
        public SearcherTargetInRadius.ResultCheck IsCorrect(HabObject target)
        {
            if (GetTeam(_me) == GetTeam(target))
                return SearcherTargetInRadius.ResultCheck.Correct;
            return SearcherTargetInRadius.ResultCheck.ToDelete ;
        }

        private Team.Typ GetTeam(HabObject target)
        {
            return target.MainDates.GetOrNull<Team>().Type;
        }

        public IPartSearchRadius Clone(HabObject parrent)
        {
            var r =new IfIsMyTeam();
            r._me = parrent;
            return r;
        }
    }
}