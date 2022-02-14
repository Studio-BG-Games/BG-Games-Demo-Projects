using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

namespace Baby_vs_Aliens
{
    public class TopUIBarView : MonoBehaviour
    {
        #region Fields

        [SerializeField] private Button _menuButton;
        [SerializeField] TMP_Text _scoreText;
        [SerializeField] Slider _healthBarSlider;
        [SerializeField] Image _heathBarFillImage;
        [SerializeField] Image[] _lifeIcons;

        #endregion


        #region UnityMethods

        private void Awake()
        {
            if (_menuButton == null)
                throw new System.Exception($"Menu Button in {typeof(TopUIBarView)} must be set up");
        }

        protected void OnDestroy()
        {
            _menuButton.onClick.RemoveAllListeners();
        }

        #endregion


        #region Methods

        public void Init(UnityAction pauseGame)
        {
            _menuButton.onClick.AddListener(pauseGame);
        }

        public void UpdateScore(int score)
        {
            _scoreText.text = score.ToString();
        }

        public void UpdateLives(int newLivesAmount)
        {
            foreach (var lifeIcon in _lifeIcons)
                lifeIcon.gameObject.SetActive(false);

            var livesCount = Mathf.Min(_lifeIcons.Length, newLivesAmount);

            for (int i = 0; i < livesCount; i++)
                _lifeIcons[i].gameObject.SetActive(true);
        }

        public void UpdateHealthBar(float percentage)
        {
            _healthBarSlider.value = percentage;
            _heathBarFillImage.color = Color.Lerp(Color.red, Color.green, percentage);
        }

        #endregion
    }
}