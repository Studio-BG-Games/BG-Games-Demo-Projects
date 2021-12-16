using UnityEngine;

namespace Gameplay.HubObject.Beh.GiverTarget.PartSearchInTarget
{
    [CreateAssetMenu(menuName = "GameSO/Chains/Radius Search/IsNotMe",order = 51)]
    public class IsNotMe_SO : IPartSearchRadius_SOWarper
    {
        [SerializeField] private IsNotMe _team;

        protected override IPartSearchRadius GetPart()
        {
            return _team;
        }
    }
}