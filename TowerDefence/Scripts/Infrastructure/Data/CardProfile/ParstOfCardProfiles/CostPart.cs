using Gameplay.UI;
using Gameplay.Units;
using UnityEngine;

namespace DefaultNamespace.Infrastructure.Data.ParstOfCardProfiles
{
    [CreateAssetMenu(menuName = "GameSO/Profile/Part/Cost")]
    public class CostPart : PartOfCardProfile
    {
        public int Cost => _cost;
        [Min(0)][SerializeField] private int _cost;
    }
}