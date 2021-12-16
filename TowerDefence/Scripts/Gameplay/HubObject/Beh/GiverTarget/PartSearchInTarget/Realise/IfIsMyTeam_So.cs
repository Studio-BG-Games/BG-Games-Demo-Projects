using UnityEngine;

namespace Gameplay.HubObject.Beh.GiverTarget.PartSearchInTarget
{
    [CreateAssetMenu(menuName = "GameSO/Chains/Radius Search/IfIsMyTeam",order = 51)]
    public class IfIsMyTeam_So : IPartSearchRadius_SOWarper
    {
        [SerializeField] private IfIsMyTeam _ifIsMyTeam;
        protected override IPartSearchRadius GetPart() => _ifIsMyTeam;
    }
}