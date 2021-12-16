using System;

namespace DefaultNamespace.Infrastructure.Data
{
    [System.Serializable]
    public class DataSettingGame : ISaveData
    {
        public float Effect=1;
        public float Volume=1;
        public bool HasBlood=false;
        public bool HasVibration=false;

        public DataSettingGame()
        {
            Effect = 1;
            Volume = 1;
            HasBlood = false;
            HasVibration = false;
        }
        public string NameFile => "SettingDataOfGame";
    }
}