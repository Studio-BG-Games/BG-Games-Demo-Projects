using System;
using System.Collections.Generic;

namespace Baby_vs_Aliens
{
    [Serializable]
    public class LevelSet
    {
        #region Fields

        public List<Level> Levels = new List<Level>();
        private bool _isLoadedSuccessfully;

        #endregion


        #region Properties

        public int LevelCount => Levels.Count;
        public bool IsLoadedSuccessfully => _isLoadedSuccessfully;

        #endregion


        #region ClassLifeCycles

        public LevelSet()
        {
            AddNewLevel();
        }

        #endregion


        #region Methods

        public void AddNewLevel()
        {
            Levels.Add(new Level());
        }

        public void AddNewLevelWithSize(int width, int height)
        {
            Levels.Add(new Level(width, height));
        }

        public void RemoveLevel(int index)
        {
            if (index >= 0 || index < Levels.Count)
                Levels.RemoveAt(index);
        }

        public Level GetLevel(int index)
        {
            if (index >= 0 || index < Levels.Count)
                return Levels[index];

            return new Level();
        }

        public void ConvertlevelsToSerializable()
        {
            foreach (var level in Levels)
                level.ConvertToSerializable();
            _isLoadedSuccessfully = false;
        }

        public void ConvertFromSerialized()
        {
            foreach (var level in Levels)
                level.ConvertFromSerializable();
            _isLoadedSuccessfully = true;
        }

        public void CopyFromExistingLevelSet(LevelSet levelSet)
        {
            List<Level> newLevelsConfig = new List<Level>();

            foreach (var level in levelSet.Levels)
            {
                var width = level.LevelData.GetLength(0);
                var height = level.LevelData.GetLength(1);

                var newLevel = new Level();
                newLevel.LevelData = new ArenaObjectType[width, height];

                for (int i = 0; i < width; i++)
                    for (int j = 0; j < height; j++)
                    {
                        newLevel.LevelData[i, j] = level.LevelData[i, j];
                    }
                newLevelsConfig.Add(level);
            }

            Levels = newLevelsConfig;
        }

        public LevelSet GetDeserializedLevelSetCopy()
        {
            var levelSet = new LevelSet();
            levelSet.Levels.Clear();

            for (int i = 0; i < Levels.Count; i++)
            {
                var newLevel = new Level();
                newLevel.CopySerializedDataFromExisting(Levels[i].SerializedLevelData);

                levelSet.Levels.Add(newLevel);
            }

            levelSet.ConvertFromSerialized();

            return levelSet;
        }

        #endregion
    }
}