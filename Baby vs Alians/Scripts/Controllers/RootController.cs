using UnityEngine;

namespace Baby_vs_Aliens
{
    public class RootController : MonoBehaviour
    {
        #region Fields

        [SerializeField] Context _context;

        private MainController _mainController;

        #endregion


        #region UnityMethods

        private void Awake()
        {
            Application.targetFrameRate = 60;

            _context.LoadLevelSet();

            ServiceLocator.AddService(new ObjectPoolManager());
            ServiceLocator.AddService(_context);

            var playerProfile = new PlayerProfile();
            playerProfile.CurrentState.Value = GameState.Menu;
            _mainController = new MainController(playerProfile);
        }

        private void Update()
        {
            _mainController.UpdateRegular();
        }

        protected void OnDestroy()
        {
            _mainController?.Dispose();
        }

        #endregion
    }
}