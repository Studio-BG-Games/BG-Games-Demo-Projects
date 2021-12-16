using UnityEngine;

namespace DefaultNamespace.Infrastructure.Data.ParstOfCardProfiles
{
    [CreateAssetMenu(menuName = "GameSO/Profile/Part/AvatarPart")]
    public class AvatarPart : PartOfCardProfile
    {
        public Sprite Mini => _mini;
        public Sprite Big => _big;
        
        [SerializeField] private Sprite _mini;
        [SerializeField] private Sprite _big;
    }
}