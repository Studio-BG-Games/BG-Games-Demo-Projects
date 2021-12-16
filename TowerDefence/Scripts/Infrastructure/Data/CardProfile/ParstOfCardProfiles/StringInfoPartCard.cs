using UnityEngine;

namespace DefaultNamespace.Infrastructure.Data.ParstOfCardProfiles
{
    [CreateAssetMenu(menuName = "GameSO/Profile/Part/String info")]
    public class StringInfoPartCard : PartOfCardProfile
    {
        [Header("Localization self")]
        [SerializeField] private string _idCard;
        [Header("Localization part")]
        [SerializeField] private string _nameFile;
        [SerializeField] private string _idLocalization;

        public string IdCard => _idCard;
        public string NameFile => _nameFile;
        public string IdLocalization => _idLocalization;
    }
}