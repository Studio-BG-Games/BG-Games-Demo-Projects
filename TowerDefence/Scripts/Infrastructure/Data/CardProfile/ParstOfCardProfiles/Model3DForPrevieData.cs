using UnityEngine;

namespace DefaultNamespace.Infrastructure.Data.ParstOfCardProfiles
{
    [CreateAssetMenu(menuName = "GameSO/Profile/Part/Model3DForPrevieData")]
    public class Model3DForPrevieData : PartOfCardProfile
    {
        public GameObject Model => _model;
        [SerializeField] private GameObject _model;
    }
}