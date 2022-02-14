namespace Baby_vs_Aliens
{
    public class MainMenuController : BaseController
    {
        #region Fields

        private readonly PlayerProfile _playerProfile;
        private MainMenuView _view;
        private Context _context;

        #endregion


        #region ClassLifeCycles

        public MainMenuController(PlayerProfile playerProfile)
        {
            _playerProfile = playerProfile;
            _context = ServiceLocator.GetService<Context>();
            InstantiatePrefab<MainMenuView>(_context.UIPrefabsData.MainMenu, _context.UiHolder, InitView);
        }

        #endregion


        #region

        private void InitView(MainMenuView view)
        {
            _view = view;
            _view.Init(StartGame);
        }

        private void StartGame()
        {
            if (_context.CharacterOptions.IsCharacterSelectable)
                _playerProfile.CurrentState.Value = GameState.CharacterSelection;
            else if (_context.CharacterOptions.IsCharacterCustomizeable)
                _playerProfile.CurrentState.Value = GameState.Customization;
            else
                _playerProfile.CurrentState.Value = GameState.Game;
            
        }

        #endregion
    }
}