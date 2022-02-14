using UnityEngine;
using UnityEngine.UI;

namespace Baby_vs_Aliens
{
    [CreateAssetMenu(fileName = "UIPrefabsData", menuName = "Data/UIPrefabsData")]
    public class UiPrefabsData : ScriptableObject
    {
        #region Fields

        [SerializeField] private MainMenuView _mainMenuPrefab;
        [SerializeField] private PauseMenuView _pauseMenuPrefab;
        [SerializeField] private TopUIBarView _topUiBarPrefab;
        [SerializeField] private CustomizationUIView _customizationUiPrefab;
        [SerializeField] private CharacterSelectionUiView _characterSelectionUiPrefab;
        [SerializeField] private Image _screenCover;

        #endregion


        #region Properties

        public MainMenuView MainMenu => _mainMenuPrefab;
        public PauseMenuView PauseMenu => _pauseMenuPrefab;
        public TopUIBarView TopUiBar => _topUiBarPrefab;
        public CustomizationUIView CustomizationUI => _customizationUiPrefab;
        public CharacterSelectionUiView CharacterSelectionUI => _characterSelectionUiPrefab;
        public Image ScreenCover => _screenCover;

        #endregion
    }
}
