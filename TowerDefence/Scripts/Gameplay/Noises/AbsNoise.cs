
namespace Gameplay.Noises
{
    [System.Serializable]
    public abstract class AbsNoise
    {
        public abstract float Get(int x, int y, int z);
        public abstract float Get(int x, int y);
        public abstract void SetSeed(int seed);
    }
}