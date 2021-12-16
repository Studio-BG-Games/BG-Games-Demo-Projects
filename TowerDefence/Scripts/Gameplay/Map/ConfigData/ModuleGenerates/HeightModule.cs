using System;
using Extension;
using PerlinNode;
using UnityEngine;

namespace Gameplay.Map.ConfigData.ModuleGenerates
{
    [Serializable]
    public class HeightModule : ModuleGenerate<float>
    {
        [SerializeField] private RangeBetween _rangeBetween;
        [SerializeField] private ShellMapNoise _mapHeight;

        public HeightModule(ShellMapNoise nodeContainer) => _mapHeight = nodeContainer;

        public float Min => _rangeBetween.Get(0);
        public float Max => _rangeBetween.Get(1);
        
        public override float Get(double x, double z) => _rangeBetween.Get((float)_mapHeight.GetNormal(x,z));
        
        public override void SetSeed(int seed)
        {
            _mapHeight.SetSeed(seed);
        }

        public override float Get(double x, double y, double z) => Get(x,z);
    }
}
