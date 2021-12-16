using Gameplay.Builds.Data;
using UnityEngine;

namespace Gameplay.Builds
{
    public abstract class GhostView : MonoBehaviour
    {
        [SerializeField] private MeshRenderer[] _meshRenderers;
        
        public abstract void Init(SizeOnMap sizeOnMap);

        public void SetColor(Color color)
        {
            foreach (var meshRenderer in _meshRenderers)
            {
                meshRenderer.material.color = color;
            }
        }
    }
}