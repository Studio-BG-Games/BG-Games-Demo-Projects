using System;
using System.Collections.Generic;
using Gameplay.Builds;
using Gameplay.Builds.Data.Marks;
using Gameplay.Map.ConfigData;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Gameplay.Map
{
    [CreateAssetMenu(menuName = "GameSO/MapGeneration/Biome", order = 51)]
    public class Biome : ScriptableObject
    {
        [Range(0,1f)][SerializeField] private float _temperature;
        [Range(0,1f)][SerializeField] private float _wet;
        [Range(0,1f)][SerializeField] private float _vegetation;
        [SerializeField] private ListDefaultBlock _listDefaultBlock;
        [SerializeField] private Brick _mainBrickBiome;
        [SerializeField] private List<PairPropAndBuild> _props;
        
        public float Wet => _wet;
        public float Vegetation => _vegetation;
        public float Temperature => _temperature;
        public Brick GetDefaultBlockBiomeOrNull(DefaultBrick.Types types) => _listDefaultBlock.GetOrNull(types);

        public Build GetRandomPropOrNull()
        {
            Build result = null;
            if (_props == null || _props.Count == 0)
                return result;
            var propPair = _props[Random.Range(0, _props.Count)];
            result = Random.Range(0, 1f) < propPair.Chance ? propPair.Prop : null;

            return result;
        }
        
        public Build GetRandomPropOrNull(System.Random rnd)
        {
            Build result = null;
            if (_props == null || _props.Count == 0)
                return result;
            var propPair = _props[rnd.Next(0, _props.Count)];
            result = rnd.NextDouble() < propPair.Chance ? propPair.Prop : null;

            return result;
        }

        public Brick MainBrickBiome => _mainBrickBiome;

        private void OnValidate() => _props.ForEach(x=>x.OnValidate());

        
        [System.Serializable]
        public class PairPropAndBuild
        {
            [SerializeField]private Build _prop;
            [SerializeField][Range(0,1f)]private float _chance;

            public Build Prop => _prop;
            public float Chance => _chance;

            public void OnValidate()
            {
                if(_prop==null)
                    return;
                if (_prop.MainDates.GetOrNull<PropMark>() == null)
                {
                    Debug.LogWarning($"{_prop.name} не содержит марку пропа, поэтому он удален. Добавть - {typeof(PropMark)}");
                    _prop = null;
                }
            }
        }

        
    }
}