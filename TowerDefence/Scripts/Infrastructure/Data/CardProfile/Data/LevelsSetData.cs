using UnityEngine;

namespace DefaultNamespace.Infrastructure.Data
{
    [System.Serializable]
    public class LevelsSetData : SaveDataProfile
    {
        [SerializeField] public int CurrentIndexLevel = 0;
        [SerializeField] public bool IsCompletedSet = false;
    }
}