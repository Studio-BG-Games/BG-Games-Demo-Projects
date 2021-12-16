using System;
using UnityEditor;
using UnityEngine;

namespace Gameplay.Map.ConfigData
{
    [CreateAssetMenu(menuName = "GameSO/MapGeneration/World Setting", order = 51)]
    public class DataMapSo : ScriptableObject
    {
        public DataMap Value => value;
        [SerializeField] private DataMap value;
    }
}