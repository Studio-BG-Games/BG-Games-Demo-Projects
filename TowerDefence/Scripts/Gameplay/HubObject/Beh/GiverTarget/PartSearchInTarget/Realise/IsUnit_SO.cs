using UnityEngine;

namespace Gameplay.HubObject.Beh.GiverTarget.PartSearchInTarget
{
    [CreateAssetMenu(menuName = "GameSO/Chains/Radius Search/IsUnit",order = 51)]
    public class IsUnit_SO : IPartSearchRadius_SOWarper
    {
        [SerializeField] private IsUnit _team;

        protected override IPartSearchRadius GetPart()
        {
            return _team;
        }
    }
}