using System.Collections.Generic;
using Mechanics.GameLevel.Stages;
using Mechanics.Garage;
using Mechanics.Interfaces;
using UnityEngine;

namespace Infrastructure.Configs
{
    [CreateAssetMenu(menuName = "Config/Config Level", order = 51)]
    public class ConfigLevel : ScriptableObject
    {
        [Range(1,3)]public int Difficulty = 1;
        public Sprite Stiker;
        public Car CarTemplate;
        public List<StageData> StagesLevel = new List<StageData>(); 
    }
}