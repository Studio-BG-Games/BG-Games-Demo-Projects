using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Map
{
    public class Brick : MonoBehaviour
    {
        public Biome BiomeBlock => _biome;
        public bool IsWater => _isWater;
        public string ID => _id;
        public List<MeshRenderer> MeshRenderers => _meshRenderers != null ? _meshRenderers : _meshRenderers = CreateList();

        private List<MeshRenderer> CreateList()
        {
            List<MeshRenderer> result = new List<MeshRenderer>();
            result.Add(_up);
            result.Add(_down);
            result.Add(_right);
            result.Add(_left);
            result.Add(_front);
            result.Add(_back);
            return result;
        }

        private List<MeshRenderer> _meshRenderers;

        [SerializeField] private Biome _biome;
        [SerializeField][HideInInspector]private string _id;
        [SerializeField] private MeshRenderer _up;
        [SerializeField] private MeshRenderer _down;
        [SerializeField] private MeshRenderer _right;
        [SerializeField] private MeshRenderer _left;
        [SerializeField] private MeshRenderer _front;
        [SerializeField] private MeshRenderer _back;
        
        
        [SerializeField] private bool _isWater;

        public void Off() => MeshRenderers.ForEach(x =>
        {
            if(x)
                x.gameObject.SetActive(false);
        });
        
        public void Init(Biome biome, bool isWater)
        {
            _biome = biome;
            _isWater = isWater;
        }

        public void UpdateMeshByHeight(float front, float back, float left, float right)
        {
            Destroy(_down.gameObject);
            var myH = transform.position.y;
            if(right>=myH || _isWater)
                DestroyImmediate(_right.gameObject);
            if(left>=myH || _isWater)
                DestroyImmediate(_left.gameObject);
            if(front>=myH || _isWater)
                DestroyImmediate(_front.gameObject);
            if(back>=myH || _isWater)
                DestroyImmediate(_back.gameObject);
        }
    }
}