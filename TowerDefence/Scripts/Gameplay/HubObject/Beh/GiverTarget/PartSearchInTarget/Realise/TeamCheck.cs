using System.Collections.Generic;
using BehTreeBrick.Action;
using Gameplay.HubObject.Data;
using Plugins.HabObject;
using UnityEngine;

namespace Gameplay.HubObject.Beh.GiverTarget.PartSearchInTarget
{
    [System.Serializable]
    public class TeamCheck : IPartSearchRadius
    {
        [SerializeField] private List<Team.Typ> _correctTeams = new List<Team.Typ>();
        
        public SearcherTargetInRadius.ResultCheck IsCorrect(HabObject target)
        {
            if (_correctTeams.Contains(target.MainDates.GetOrNull<Team>().Type))
                return SearcherTargetInRadius.ResultCheck.Correct;
            return SearcherTargetInRadius.ResultCheck.ToDelete;
        }

        public IPartSearchRadius Clone(HabObject parrent)
        {
            var r = new TeamCheck();
            r._correctTeams = _correctTeams;
            return r;
        }
    }
}