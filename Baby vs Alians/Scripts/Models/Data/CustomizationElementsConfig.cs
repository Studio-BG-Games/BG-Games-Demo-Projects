using UnityEngine;

namespace Baby_vs_Aliens
{
    [CreateAssetMenu(fileName = "CustomizationElementsConfig", menuName = "Data/CustomizationElementsConfig")]
    public class CustomizationElementsConfig : ScriptableObject
    {
        #region Fields

        [SerializeField] private Material[] _shirtMaterials;
        [SerializeField] private Material[] _hairMaterials;
        [SerializeField] private GameObject[] _gunPrefabs;
        [SerializeField] private GameObject[] _characterModelPrefabs;
        #endregion


        #region Properties

        public Material[] ShirtMaterials => _shirtMaterials;
        public Material[] HairMaterials => _hairMaterials;
        public GameObject[] GunPrefabs => _gunPrefabs;
        public GameObject[] CharacterModelPrefabs => _characterModelPrefabs;

        #endregion
    }
}