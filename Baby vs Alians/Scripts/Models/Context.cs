using System;
using UnityEngine;

namespace Baby_vs_Aliens
{
    [Serializable]
    public class Context
    {
        #region Fields

        [SerializeField] private Transform _uiHolder;
        [SerializeField] private UiPrefabsData _uiPrefabsData;
        [SerializeField] private PlayerConfig _playerConfig;
        [SerializeField] private ArenaView _arenaPrefab;
        [SerializeField] private LevelsConfig _levelsConfig;
        [SerializeField] private CustomizationElementsConfig _customizationElementsConfig;
        [SerializeField] private CustomizationCharacterView _customizationCharacterPrefab;
        [SerializeField] private CharacterSelectionTurnTableView _selectionBasePrefab;
        [SerializeField] private CharacterOptions _characterOptions;
        [SerializeField] private bool _areLevelsLooped;
        [SerializeField] private EnemyAmount _defaultEnemyAmount;

        private LevelSet _levelSet;

        #endregion


        #region Properties

        public Transform UiHolder => _uiHolder;
        public UiPrefabsData UIPrefabsData => _uiPrefabsData;
        public PlayerConfig PlayerConfig => _playerConfig;
        public ArenaView ArenaPrefab => _arenaPrefab;
        public LevelSet LevelSet => _levelSet;
        public CustomizationElementsConfig CustomizationElementsConfig => _customizationElementsConfig;
        public CustomizationCharacterView CustomizationCharacterPrefab =>_customizationCharacterPrefab;
        public CharacterSelectionTurnTableView SelectionBasePrefab => _selectionBasePrefab;
        public CharacterOptions CharacterOptions => _characterOptions;
        public bool AreLevelsLooped => _areLevelsLooped;
        public EnemyAmount EnemyAmount => _defaultEnemyAmount;

        #endregion


        #region Methods

        public void LoadLevelSet()
        {
            try
            {
                _levelSet = _levelsConfig.LevelSet.GetDeserializedLevelSetCopy();
            }
            catch
            {
                _levelSet = new LevelSet();
            }
        }

        #endregion
    }
}