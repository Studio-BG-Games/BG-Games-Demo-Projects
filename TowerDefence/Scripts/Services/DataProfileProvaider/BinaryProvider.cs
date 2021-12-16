using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Gameplay.HubObject.Data;
using Interface;
using Plugins.HabObject;
using UnityEngine;

namespace DefaultNamespace.Infrastructure.Data
{
    public class BinaryProvider : SaveDataProvider
    {
        public static string Path = Application.persistentDataPath+"/";
        private BinaryFormatter _serilizator;

        public BinaryProvider() => _serilizator = new BinaryFormatter();

        public override TarData GetOrNull<T, Tar, TarData>(Tar habObject)
        {
            string id = "";
            if (habObject is HabObject)
                id = (habObject as HabObject).MainDates.GetOrNull<IdContainer>().ID;
            else
                id = ((IIDContainer) habObject).ID;
            var path = GetPath(id);
            if (!File.Exists(path))
                return null;
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                TarData deserilizeT = (TarData) _serilizator.Deserialize(fs);
                return deserilizeT;
            }
        }

        public override void Save<T, Tar, TarData>(Tar habObject, TarData dataToSave) 
        {
            string id = "";
            if (habObject is HabObject)
                id = (habObject as HabObject).MainDates.GetOrNull<IdContainer>().ID;
            else
                id = ((IIDContainer) habObject).ID;
            var path = GetPath(id);
            if(File.Exists(path))
                File.Delete(path);
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                _serilizator.Serialize(fs, dataToSave);
            }
        }

        public override void Save(ISaveData data)
        {
            var path = Path + data.NameFile + ".dat";
            if(File.Exists(path))
                File.Delete(path);
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                _serilizator.Serialize(fs, data);
            }
        }

        public override T GetOrDefault<T>()  
        {
            T t = new T();
            var path = Path + t.NameFile + ".dat";
            if (!File.Exists(path))
            {
                return t;
            }
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                return (T)_serilizator.Deserialize(fs) ;
            }
        }

        private string GetPath(string id) => Path + id + ".dat";

        public static void Test() => Debug.Log(Path);
    }
}