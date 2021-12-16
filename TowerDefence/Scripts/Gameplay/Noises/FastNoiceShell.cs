using UnityEngine;

namespace Gameplay.Noises
{
    [System.Serializable]
    public class FastNoiceShell :  AbsNoise
    {
        private FastNoiseLite _fastNoise;
        private FastNoiseLite FastNoise => _fastNoise ??= new FastNoiseLite();

        [Range(0.01f,1)][SerializeField] private float _frequence=0.01f;
        [Range(0,100)][SerializeField] private int _octaves =3 ;
        [Range(0,10f)][SerializeField] private float _lacunarity = 2;
        [Range(0,1f)][SerializeField] private float _weightedStrength;
        [Range(0, 5f)] [SerializeField] private float _pingPongStrength;
        [SerializeField] private FastNoiseLite.NoiseType _noiseType;
        [SerializeField] private FastNoiseLite.FractalType _fractalType;
        [SerializeField] private int _seed = 1337;

        public override float Get(int x, int y) => (FastNoise.GetNoise(x, y) + 1) / 2;
        public override float Get(int x, int y, int z) => (FastNoise.GetNoise(x, y, z) + 1) / 2;
        public override void SetSeed(int seed) => FastNoise.SetSeed(seed);

        private void UpdateFastNoiseState()
        {
            FastNoise.SetSeed(_seed);
            FastNoise.SetFrequency(_frequence);
            FastNoise.SetNoiseType(_noiseType);
            FastNoise.SetFractalType(_fractalType);
            FastNoise.SetFractalOctaves(_octaves);
            FastNoise.SetFractalLacunarity(_lacunarity);
            FastNoise.SetFractalWeightedStrength(_weightedStrength);
            FastNoise.SetFractalPingPongStrength(_pingPongStrength);
        }
    }
}