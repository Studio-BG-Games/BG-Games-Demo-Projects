
using System;
using System.Collections.Generic;
using System.Linq;
using PerlinNode;
using UnityEngine;

namespace Gameplay.Map.ConfigData.ModuleGenerates
{
    [Serializable]
    public class BiomeModule : ModuleGenerate<Biome>
    {
        [SerializeField] private ShellMapNoise _mapWet;
        [SerializeField] private ShellMapNoise _mapTemperature;
        [SerializeField] private ShellMapNoise _mapVegetation;
        [SerializeField] private BiomeShell _biomes;
        
        public BiomeModule(ShellMapNoise mapWet, ShellMapNoise mapVegetation, ShellMapNoise mapTemperature)
        {
            _mapWet = mapWet;
            _mapTemperature = mapTemperature;
            _mapVegetation = mapVegetation;
        }

        public override Biome Get(double x, double y)
        {
            double wet = _mapWet.GetNormal(x, y);
            double temperature = _mapTemperature.GetNormal(x, y);
            double vegetation = _mapVegetation.GetNormal(x, y);
            return _biomes.GetBiome(temperature, wet, vegetation);
        }

        public override void SetSeed(int seed)
        {
            _mapTemperature.SetSeed(seed);
            _mapVegetation.SetSeed(seed);
            _mapWet.SetSeed(seed);
        }

        public override Biome Get(double x, double y, double z)
        {
            double wet = _mapWet.GetNormal(x, y,z);
            double temperature = _mapTemperature.GetNormal(x, y,z);
            double vegetation = _mapVegetation.GetNormal(x, y,z);
            return _biomes.GetBiome(temperature, wet, vegetation);
        }
        
        [Serializable]
        private class BiomeShell
        {
            [SerializeField] private List<Biome> _biomes;
            [NonSerialized] private Biome _maxBiome; 
            
            public void Sort()
            {
                foreach (var b in _biomes.OrderBy( x => x.Temperature).ThenBy(x => x.Wet).ThenBy(x => x.Vegetation).ToList()) Debug.Log($"{b.name}; t:{b.Temperature}; w:{b.Wet}; v:{b.Vegetation}");
                Debug.Log("_________");
                _maxBiome = _biomes.OrderByDescending(x => x.Temperature + x.Vegetation + x.Wet).First();
                Debug.Log($"{_maxBiome.name}; t:{_maxBiome.Temperature}; w:{_maxBiome.Wet}; v:{_maxBiome.Vegetation}");
            }

            public Biome GetBiome(double temepature, double wet, double vegetation)
            {
                if (_maxBiome == null)
                    Sort();
                var result =
                    _biomes.Where(x => temepature <= x.Temperature).OrderBy(x => x.Wet).
                        Where(x => wet <= x.Wet).OrderBy(x => vegetation <= x.Vegetation)
                        .FirstOrDefault();
                if (result == null)
                    result = _maxBiome;
                return result;
            }
        }
    }
}
