using System;
using UnityEngine;

namespace Baby_vs_Aliens
{ 
    [Serializable]
    public struct EnemyPrefab
    {
        public EnemyType EnemyType;
        public GameObject Prefab;
    }
}