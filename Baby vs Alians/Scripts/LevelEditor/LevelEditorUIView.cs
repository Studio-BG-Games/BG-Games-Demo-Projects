using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Baby_vs_Aliens.LevelEditor
{
    public class LevelEditorUIView : MonoBehaviour
    {
        #region Fields

        [SerializeField] private List<Toggle> _toggles;
        [SerializeField] private TMP_Dropdown _levelSelectionDropdown;
        private Dictionary<Toggle, ArenaObjectType> _typesByToggle;

        [SerializeField] private Button _addLevelButton;
        [SerializeField] private Button _deleteLevelButton;
        [SerializeField] private Button _newLevelSetButton;
        [SerializeField] private Button _saveLevelSetButton;
        [SerializeField] private Button _loadLevelSetButton;

        #endregion

        #region Properties

        public Button AddLevelButton => _addLevelButton;
        public Button DeleteLevelButton => _deleteLevelButton;
        public Button NewLevelSetButton => _newLevelSetButton;
        public Button SaveLevelSetButton => _saveLevelSetButton;
        public Button LoadLevelSetButton => _loadLevelSetButton;
        public TMP_Dropdown LevelSelectionDropdown => _levelSelectionDropdown;

        public bool HasToggleSelected => _toggles.Find(x => x.isOn) != null;
        public ArenaObjectType SelectedObjectType => _typesByToggle[_toggles.Find(x => x.isOn)];

        #endregion

        private void Awake()
        {
            SetUpToggles();
        }

        private void SetUpToggles()
        {
            foreach (var toggle in _toggles)
                toggle.onValueChanged.AddListener((call) => OnToggleChange(toggle, call));

            _typesByToggle = new Dictionary<Toggle, ArenaObjectType>();
            _typesByToggle.Add(_toggles[0], ArenaObjectType.ObstacleHor);
            _typesByToggle.Add(_toggles[1], ArenaObjectType.ObstacleVer);
            _typesByToggle.Add(_toggles[2], ArenaObjectType.PlayerSpawn);
            _typesByToggle.Add(_toggles[3], ArenaObjectType.BigEnemySpawn);
            _typesByToggle.Add(_toggles[4], ArenaObjectType.SmallEnemySpawn);
        }

        private void OnToggleChange(Toggle toggle, bool value)
        {
            if (value)
                foreach (var t in _toggles)
                    if (t != toggle)
                        t.isOn = false;
        }

        public void FillDropdownOptions(int levelCount, int currentLevelIndex)
        {
            List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();

            for (int i = 0; i < levelCount; i++)
            {
                options.Add(new TMP_Dropdown.OptionData($"Level_{ i + 1 }"));
            }

            _levelSelectionDropdown.ClearOptions();
            _levelSelectionDropdown.AddOptions(options);

            _levelSelectionDropdown.value = currentLevelIndex;
        }
    }
}