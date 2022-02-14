using UnityEngine;

namespace Baby_vs_Aliens
{
    public class CustomizationCharacterView : MonoBehaviour
    {
        [SerializeField] private Transform _modelHolder;
        private Transform _gunBone;
        private SkinnedMeshRenderer _meshRenderer;

        private const int SHIRT_MATERIAL_INDEX = 5;
        private const int HAIR_MATERIAL_INDEX = 9;

        public void Init(GameObject gun, Material shirtMaterial, Material hairMaterial)
        {
            var children = GetComponentsInChildren<Transform>();

            foreach (var child in children)
                if (child.CompareTag("GunHolder"))
                {
                    _gunBone = child;
                    break;
                }

            foreach (var child in children)
                if (child.CompareTag("DefaultGun"))
                {
                    child.gameObject.SetActive(false);
                    break;
                }

            SetGun(gun);

            SetShirtMaterial(shirtMaterial);

            SetHairMaterial(hairMaterial);
        }

        public void InitCharacterModel(GameObject characterModel)
        {
            characterModel.transform.parent = _modelHolder.transform;
            characterModel.transform.localPosition = Vector3.zero;
            characterModel.transform.localRotation = Quaternion.identity;

            _meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        }

        public void SetHairMaterial(Material hairMaterial)
        {
            SetMaterial(hairMaterial, HAIR_MATERIAL_INDEX);
        }

        public void SetShirtMaterial(Material shirtMaterial)
        {
            SetMaterial(shirtMaterial, SHIRT_MATERIAL_INDEX);
        }

        private void SetMaterial(Material material, int materialIndex)
        {
            if (materialIndex <= 0 || materialIndex > _meshRenderer.materials.Length - 1)
                return;

            var materials = _meshRenderer.materials;

            materials[materialIndex] = material;

            _meshRenderer.materials = materials;
        }

        public void SetGun(GameObject gun)
        {
            gun.transform.parent = _gunBone;
            gun.transform.localPosition = Vector3.zero;
            gun.transform.localRotation = Quaternion.identity;
        }
    }
}