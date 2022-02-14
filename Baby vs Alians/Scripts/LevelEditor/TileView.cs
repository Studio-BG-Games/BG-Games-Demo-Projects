using System.Collections.Generic;
using UnityEngine;

namespace Baby_vs_Aliens.LevelEditor
{
    [RequireComponent(typeof (MeshRenderer))]
    public class TileView : MonoBehaviour
    {
        [SerializeField] private List<TileMaterial> _materials;

        private MeshRenderer _meshRenderer;
        private Material _defaultMaterial;

        private void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
            _defaultMaterial = _meshRenderer.material;
        }

        public void SetMaterialByType(ArenaObjectType type)
        {
            var newMaterial = _materials.Find(x => x.Type == type).Material;
            _meshRenderer.material = newMaterial != null ? newMaterial : _defaultMaterial;
        }
    }
}