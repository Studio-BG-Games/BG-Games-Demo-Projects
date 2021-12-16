using System;
using BehTreeBrick.Action;
using Gameplay.HubObject.Data;
using Plugins.HabObject;
using UnityEngine;

namespace Gameplay.HubObject.Beh.GiverTarget.PartSearchInTarget
{
    [System.Serializable]
    public class IfIsNotMyTeam: IPartSearchRadius
    {
         private HabObject _me;
        
        public SearcherTargetInRadius.ResultCheck IsCorrect(HabObject target)
        {
            if (GetTeam(target)
                != 
                GetTeam(_me))
                return SearcherTargetInRadius.ResultCheck.Correct;
            return SearcherTargetInRadius.ResultCheck.ToDelete ;
        }

        private Team.Typ GetTeam(HabObject target)
        {
            return target.MainDates.GetOrNull<Team>().Type;    
        }

        public IPartSearchRadius Clone(HabObject parrent)
        {
            var r =new IfIsNotMyTeam();
            r._me = parrent;
            return r;
        }
    }
}