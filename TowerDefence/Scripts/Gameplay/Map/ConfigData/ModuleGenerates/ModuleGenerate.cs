using System;
using PerlinNode;
using UnityEngine;

namespace Gameplay.Map.ConfigData.ModuleGenerates
{
    [Serializable]
    public abstract class ModuleGenerate<T>
    {
        public abstract T Get(double x, double z);

        public abstract void SetSeed(int seed);
        
        public abstract T Get(double x, double y, double z);
    }
}