#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Baby_vs_Aliens.LevelEditor
{
    public class LevelSetUnpacker : MonoBehaviour
    {
        [SerializeField] private LevelsConfig _outputLevelsConfig;
        [SerializeField] private Button _unpackButton;

        private SaveDataRepository _saveDataRepository;

        private void Awake()
        {
            _unpackButton.onClick.AddListener(UnpackLevelSet);
            _saveDataRepository = new SaveDataRepository();
        }

        private void UnpackLevelSet()
        {
            if (!_outputLevelsConfig)
                return;

            var levelSet = new LevelSet();
            _saveDataRepository.LoadWithDialogue(ref levelSet);

            if (levelSet.IsLoadedSuccessfully)
            {
                _outputLevelsConfig.LevelSet.CopyFromExistingLevelSet(levelSet);
                _outputLevelsConfig.LevelSet.ConvertFromSerialized();
            }

            EditorUtility.SetDirty(_outputLevelsConfig);
        }
    }
}
#endif