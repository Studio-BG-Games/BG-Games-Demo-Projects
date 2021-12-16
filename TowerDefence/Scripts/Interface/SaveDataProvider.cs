using Plugins.HabObject;
using UnityEngine;

namespace DefaultNamespace.Infrastructure.Data
{
    public abstract class SaveDataProvider
    {
        public const string OnlineID = "OnlineTypeProvider";
        public const string LocalID = "LocalTypeProvider";
        
        public abstract TarData GetOrNull<T, Tar, TarData>(Tar habObject) 
            where T : ObjectCardProfile<Tar, TarData> where Tar : Object where TarData : SaveDataProfile;

        public abstract void Save<T, Tar, TarData>(Tar habObject, TarData dataToSave)
            where T : ObjectCardProfile<Tar, TarData> where Tar : Object where TarData : SaveDataProfile;

        public abstract void Save(ISaveData data);

        public abstract T GetOrDefault<T>() where T : ISaveData, new();
    }
    
    
}