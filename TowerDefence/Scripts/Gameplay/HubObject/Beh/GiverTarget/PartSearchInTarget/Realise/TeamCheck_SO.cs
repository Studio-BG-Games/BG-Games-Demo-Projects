using UnityEngine;

namespace Gameplay.HubObject.Beh.GiverTarget.PartSearchInTarget
{
    [CreateAssetMenu(menuName = "GameSO/Chains/Radius Search/TeamCheck",order = 51)]
    public class TeamCheck_SO : IPartSearchRadius_SOWarper
    {
        [SerializeField] private TeamCheck _team;

        protected override IPartSearchRadius GetPart()
        {
            return _team;
        }
    }
}