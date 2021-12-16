using UnityEngine;
using Random = UnityEngine.Random;

namespace Gameplay.Noises
{
    [System.Serializable]
    public class PerlinNoise : AbsNoise
    {
        [Range(0,1)][SerializeField] private float _detail;
        [Range(0,2)][SerializeField] private float _scale;
        [SerializeField] private Vector3Int _offset;
        [SerializeField] private int _seed;

        public override float Get(int x, int y, int z)
        {
            Debug.LogWarning("Perlin noise from video can't make noise in 3d");
            return Get(x, y);
        }

        public override float Get(int x, int y) => Mathf.PerlinNoise(MakeCord(x, _offset.x), MakeCord(y, _offset.y));

        private float MakeCord(int x, int offsetX) => (float) x * _detail * _scale + offsetX;

        public override void SetSeed(int seed)
        {
            var prevSeed = Random.seed;
            Random.InitState(seed);
            _offset.x = Random.Range(-100000, 100000);
            _offset.y = Random.Range(-100000, 100000);
            _offset.z = Random.Range(-100000, 100000);
            Random.InitState(prevSeed);
        }
    }
}