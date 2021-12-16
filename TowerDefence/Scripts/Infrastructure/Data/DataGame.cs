using UnityEngine;

namespace DefaultNamespace.Infrastructure.Data
{
    [System.Serializable]
    public class DataGame : ISaveData
    {
        public int MainMoney = 0;
        
        public string[] BuildPull = new string[0];
        public string[] UnitPull = new string[0];

        public DataGame()
        {
            MainMoney = 0;
            BuildPull = new string[0];
            UnitPull = new string[0];
        }
        
        public string NameFile => "GameData";
    }
}