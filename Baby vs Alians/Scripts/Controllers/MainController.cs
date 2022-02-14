namespace Baby_vs_Aliens
{
    public class MainController : BaseController, IUpdateableRegular
    {
        #region Fields

        private PlayerProfile _playerProfile;
        private GameplayController _gameController;
        private MainMenuController _mainController;
        private CustomizationController _customizationController;
        private CharacterSelectionController _characterSelectionController;

        #endregion


        #region ClassLifeCycles

        public MainController(PlayerProfile playerProfile)
        {
            _playerProfile = playerProfile;
            OnChangeGameState(_playerProfile.CurrentState.Value);
            playerProfile.CurrentState.SubscribeOnChange(OnChangeGameState);
        }

        #endregion


        #region Methods

        private void OnChangeGameState(GameState state)
        {
            switch (state)
            {
                case GameState.Menu:
                    {
                        _gameController?.Dispose();
                        _customizationController?.Dispose();
                        _characterSelectionController?.Dispose();
                        _mainController = new MainMenuController(_playerProfile);
                        break;
                    }

                case GameState.Game:
                    {
                        _mainController?.Dispose();
                        _customizationController?.Dispose();
                        _characterSelectionController?.Dispose();
                        _gameController = new GameplayController(_playerProfile);
                        break;
                    }

                case GameState.Customization:
                    {
                        _mainController?.Dispose();
                        _gameController?.Dispose();
                        _characterSelectionController?.Dispose();
                        _customizationController = new CustomizationController(_playerProfile);
                        break;
                    }

                case GameState.CharacterSelection:
                    {
                        _mainController?.Dispose();
                        _gameController?.Dispose();
                        _customizationController?.Dispose();
                        _characterSelectionController = new CharacterSelectionController(_playerProfile);
                        break;
                    }

                default:
                    _mainController?.Dispose();
                    _gameController?.Dispose();
                    _customizationController?.Dispose();
                    _characterSelectionController?.Dispose();
                    break;
            }
        }

        #endregion


        #region IDisposeable

        protected override void OnDispose()
        {
            _playerProfile.CurrentState.UnSubscribeOnChange(OnChangeGameState);
            _mainController?.Dispose();
            _gameController?.Dispose();
            _customizationController?.Dispose();
            _characterSelectionController?.Dispose();
            base.OnDispose();
        }

        #endregion


        #region IUpdateableRegular

        public void UpdateRegular()
        {
            _gameController?.UpdateRegular();
        }

        #endregion
    }
}