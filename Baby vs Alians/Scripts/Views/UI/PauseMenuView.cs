using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Baby_vs_Aliens
{
    public class PauseMenuView : MonoBehaviour
    {
        #region Fields

        [SerializeField] private Button _resumeButton;
        [SerializeField] private Button _backToMenuButton;
        [SerializeField] private Button _exitButton;

        #endregion


        #region UnityMethods

        private void Awake()
        {
            if (_resumeButton == null)
                throw new System.Exception($"Resume Button in {typeof(PauseMenuView)} must be set up");

            if (_backToMenuButton == null)
                throw new System.Exception($"Back To Menu Button in {typeof(PauseMenuView)} must be set up");

            if (_resumeButton == null)
                throw new System.Exception($"ExitButton in {typeof(PauseMenuView)} must be set up");
        }

        protected void OnDestroy()
        {
            _resumeButton.onClick.RemoveAllListeners();
            _backToMenuButton.onClick.RemoveAllListeners();
            _exitButton.onClick.RemoveAllListeners();

        }

        #endregion


        #region Methods

        public void Init(UnityAction resumeGame, UnityAction backToMenu)
        {
            _resumeButton.onClick.AddListener(resumeGame);
            _backToMenuButton.onClick.AddListener(backToMenu);
            _exitButton.onClick.AddListener(ExitGame);
        }

        public void ExitGame()
        {
#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#else
            Application.Quit();
#endif
        }

        #endregion
    }
}