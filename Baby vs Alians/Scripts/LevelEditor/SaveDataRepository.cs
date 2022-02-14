using SFB;
using System.IO;

namespace Baby_vs_Aliens.LevelEditor
{
    public class SaveDataRepository
    {
        private readonly IData<LevelSet> _levelSetData;
        private string _path;
        private const string FILE_EXTENTION = "levelset";

        public SaveDataRepository()
        {
            _levelSetData = new JsonData<LevelSet>();
        }

        public void Save(LevelSet levelSet)
        {
            _path = StandaloneFileBrowser.SaveFilePanel("Save Level Set", "", "", FILE_EXTENTION);

            if (string.IsNullOrEmpty(_path))
                return;

            levelSet.ConvertlevelsToSerializable();
            _levelSetData.Save(levelSet, _path);
        }

        public void LoadWithDialogue(ref LevelSet levelSet)
        {
            var paths = StandaloneFileBrowser.OpenFilePanel("Open File", "", FILE_EXTENTION, false);
            if (paths.Length <= 0)
                return;
            _path = paths[0];

            Load(ref levelSet);
        }

        public void LoadFromPath(string path, ref LevelSet levelSet)
        {
            _path = path;

            Load(ref levelSet);
        }

        private void Load(ref LevelSet levelSet)
        {
            var file = _path;
            if (!File.Exists(file)) return;

            var newLevelSet = _levelSetData.Load(file);

            if (newLevelSet != null)
            {
                newLevelSet.ConvertFromSerialized();
                levelSet = newLevelSet;
            }
        }
    }
}