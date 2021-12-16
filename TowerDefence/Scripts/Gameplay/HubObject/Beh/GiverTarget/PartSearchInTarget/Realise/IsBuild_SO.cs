using UnityEngine;

namespace Gameplay.HubObject.Beh.GiverTarget.PartSearchInTarget
{
    [CreateAssetMenu(menuName = "GameSO/Chains/Radius Search/IfBuild",order = 51)]
    public class IsBuild_SO : IPartSearchRadius_SOWarper
    {
        [SerializeField] private IsBuild _team;

        protected override IPartSearchRadius GetPart()
        {
            return _team;
        }
    }
}