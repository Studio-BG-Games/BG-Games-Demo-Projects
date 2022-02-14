using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MainMenuView : MonoBehaviour
{
    #region Fields

    [SerializeField] private Button _startButton;
    [SerializeField] private Button _exitButton;

    #endregion


    #region UnityMethods

    private void Awake()
    {
        if (_startButton == null)
            throw new System.Exception($"Start Button in {typeof(MainMenuView)} must be set up");

        if (_startButton == null)
            throw new System.Exception($"Exit Button in {typeof(MainMenuView)} must be set up");
    }

    protected void OnDestroy()
    {
        _startButton.onClick.RemoveAllListeners();
        _exitButton.onClick.RemoveAllListeners();

    }

    #endregion


    #region Methods

    public void Init(UnityAction startGame)
    {
        _startButton.onClick.AddListener(startGame);
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
