using UnityEngine;

namespace Baby_vs_Aliens
{
    [CreateAssetMenu(fileName = "LevelsConfig", menuName = "Data/LevelsConfig")]
    public class LevelsConfig : ScriptableObject
    {
        public LevelSet LevelSet;
    }
}