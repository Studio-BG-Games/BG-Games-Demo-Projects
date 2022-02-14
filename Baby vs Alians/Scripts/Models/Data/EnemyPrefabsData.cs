using System.Collections.Generic;
using UnityEngine;

namespace Baby_vs_Aliens
{
    [CreateAssetMenu(fileName = "EnemyPrefabsData", menuName = "Data/EnemyPrefabsData")]
    public class EnemyPrefabsData : ScriptableObject
    {
        [SerializeField] List<EnemyPrefab> _enemyPrefabs;

        public GameObject GetPrefabByType(EnemyType type)
        {
            return _enemyPrefabs.Find(x => x.EnemyType == type).Prefab;
        }
    }
}