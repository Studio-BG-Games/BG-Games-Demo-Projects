using System;
using PerlinNode;
using UnityEngine;

namespace Gameplay.Map.ConfigData.ModuleGenerates
{
    [Serializable]
    public class ShellMapNoise
    {
        [SerializeField] private double _scale1=1;
        [SerializeField] private double _scale2=1;
        [SerializeField] private NodeContainerAsset _mapHeight;

        public double GetNormal(double x, double z) 
            => (_mapHeight.GetValue(x * _scale1 * _scale2, 0, z * _scale1 * _scale2)+1)/2;
        
        public double GetNormal(double x, double y, double z) 
            => (_mapHeight.GetValue(x * _scale1 * _scale2, y*_scale1*_scale2, z * _scale1 * _scale2)+1)/2;

        public void SetSeed(int seed) => _mapHeight.SetSeed(seed);
    }
}