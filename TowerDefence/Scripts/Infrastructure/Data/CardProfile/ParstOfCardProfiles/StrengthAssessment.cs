using UnityEngine;

namespace DefaultNamespace.Infrastructure.Data.ParstOfCardProfiles
{
    [CreateAssetMenu(menuName = "GameSO/Profile/Part/Strength Assessment")]
    public class StrengthAssessment : PartOfCardProfile
    {
        public int Value => _value;
        [SerializeField][Range(1,5)]private int _value;
        public static int Max => 5;
    }
}