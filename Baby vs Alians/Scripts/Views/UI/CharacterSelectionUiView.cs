using UnityEngine;
using UnityEngine.UI;

namespace Baby_vs_Aliens
{
    public class CharacterSelectionUiView : MonoBehaviour
    {
        #region Fields

        [SerializeField] private Button _startButton;
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _nextButton;
        [SerializeField] private Button _prevButton;

        #endregion


        #region Properties

        public Button StartButton => _startButton;
        public Button BackButton => _backButton;
        public Button NextButton => _nextButton;
        public Button PrevButton => _prevButton;

        #endregion
    }
}