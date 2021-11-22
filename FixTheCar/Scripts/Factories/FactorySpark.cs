using System;
using System.Collections.Generic;
using Mechanics.GameLevel.Stages;
using Mechanics.GameLevel.Stages.NumbetStageParts.Spark;
using Plugins.DIContainer;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Factories
{
    public class FactorySpark : MonoBehaviour
    {
        [SerializeField] private List<Pair> _pairsSparkAndType;

        private DiBox _diBox = DiBox.MainBox;
        
        public Spark Create(ShadowSpark.TypeSpark type)
        {
            Spark result = null;

            foreach (var pair in _pairsSparkAndType)
            {
                if(type == pair.Type)
                {
                    result = _diBox.CreatePrefab(pair.Sparks[Random.Range(0, pair.Sparks.Count)]);
                    break;
                }
            }

            if (result) return result;
            else  throw new Exception("Factory don't have - " + type.ToString());
        }
        
        [System.Serializable]
        private class Pair
        {
            public List<Spark> Sparks;
            public ShadowSpark.TypeSpark Type;
        }
    }
}