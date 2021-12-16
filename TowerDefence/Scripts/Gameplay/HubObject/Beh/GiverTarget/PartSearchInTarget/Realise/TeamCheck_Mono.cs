using UnityEngine;

namespace Gameplay.HubObject.Beh.GiverTarget.PartSearchInTarget
{
    public class TeamCheck_Mono : IPartSearchRadius_MonoWarper
    {
        [SerializeField] private TeamCheck _teamCheck;
        
        protected override IPartSearchRadius GetPart()
        {
            return _teamCheck;
        }
    }
}