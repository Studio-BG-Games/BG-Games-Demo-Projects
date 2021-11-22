using UnityEngine;

namespace Infrastructure.Configs
{
    [CreateAssetMenu(menuName = "Config/level names", order = 51)]
    public class ConfigLevelName : ScriptableObject
    {
        public string MainMnu;
        public string Garage;
        public string GameLevel;
        public string SketchBook;
    }
}