using UnityEngine;

namespace Gameplay.HubObject.Beh.GiverTarget.PartSearchInTarget
{
    [CreateAssetMenu(menuName = "GameSO/Chains/Radius Search/Container",order = 51)]
    public class ContainerPartSearch_SO : IPartSearchRadius_SOWarper
    {
        [SerializeField] private ContainerPartSearch _containerPartSearch;
        
        protected override IPartSearchRadius GetPart() => _containerPartSearch;
    }
}