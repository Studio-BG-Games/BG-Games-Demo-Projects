using UnityEngine;

namespace Gameplay.HubObject.Beh.GiverTarget.PartSearchInTarget
{
    [CreateAssetMenu(menuName = "GameSO/Chains/Radius Search/IfIsNotMyTeam",order = 51)]
    public class IfIsNotMyTeam_SO : IPartSearchRadius_SOWarper
    {
        [SerializeField] private IfIsNotMyTeam _team;

        protected override IPartSearchRadius GetPart()
        {
            return _team;
        }
    }
}