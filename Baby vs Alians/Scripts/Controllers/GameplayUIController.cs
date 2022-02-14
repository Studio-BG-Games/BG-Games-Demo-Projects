using System;
using UnityEngine;
using UnityEngine.UI;

namespace Baby_vs_Aliens
{
    public class GameplayUIController : BaseController, IUpdateableRegular
    {
        #region Fields

        private readonly PlayerProfile _playerProfile;
        private PauseMenuView _pauseView;
        private TopUIBarView _topUIBarView;
        private Image _screenCover;
        private Context _context;
        private InputController _inputController;
        private bool _isPauseMenuOpened;

        private Action _onFadeInfinished;
        private Action _onFadeOutfinished;
        private bool _isFadingIn;
        private bool _isFadingOut;
        private Color _opaque;
        private Color _transparent;
        private float _fadeRatio;

        private Action _unsubsribeHealthTracking;

        public SubscriptionProperty<int> _score = new SubscriptionProperty<int>();

        #endregion

        #region ClassLifeCycles

        public GameplayUIController(PlayerProfile playerProfile, InputController inputController)
        {
            _playerProfile = playerProfile;
            _inputController = inputController;
            _context = ServiceLocator.GetService<Context>();

            _inputController.EscapeInput += ProcessPauseInput;

            InstantiatePrefab<TopUIBarView>(_context.UIPrefabsData.TopUiBar, _context.UiHolder, InitTopUiBarView);
            InstantiatePrefab<PauseMenuView>(_context.UIPrefabsData.PauseMenu, _context.UiHolder, InitPauseView);
            InstantiatePrefab<Image>(_context.UIPrefabsData.ScreenCover, _context.UiHolder, InitScreenCover);

            _score.Value = 0;
        }

        #endregion


        #region IUpdateableRegular

        public void UpdateRegular()
        {
            if (!(_isFadingIn || _isFadingOut))
                return;

            var delta = 0f;
            if (_isFadingIn)
                delta = Time.unscaledDeltaTime;
            if(_isFadingOut)
                delta = -Time.unscaledDeltaTime;

            _fadeRatio = Mathf.Clamp(_fadeRatio += delta, 0, 1);
            _screenCover.color = Color.Lerp(_opaque, _transparent, _fadeRatio);

            if (_isFadingIn && _fadeRatio >= 1)
                FinishFadingIn();

            if (_isFadingOut && _fadeRatio <= 0)
                FinishFadingOut();
        }

        #endregion


        #region Methods

        private void InitPauseView(PauseMenuView view)
        {
            _pauseView = view;
            _pauseView.Init(() => SetPause(false), BackToMainMenu);
            _pauseView.gameObject.SetActive(false);
        }

        private void InitTopUiBarView(TopUIBarView view)
        {
            _topUIBarView = view;
            _topUIBarView.Init(() => SetPause(true));

            _score.SubscribeOnChange(_topUIBarView.UpdateScore);
        }

        private void InitScreenCover(Image screenCover)
        {
            _screenCover = screenCover;
            _opaque = _screenCover.color;
            _opaque.a = 1;
            _transparent = _screenCover.color;
            _transparent.a = 0;
        }

        public void BeginFadeIn(Action callback)
        {
            _onFadeInfinished = callback;
            _isFadingIn = true;
            _isFadingOut = false;
            _screenCover.gameObject.SetActive(true);
        }

        public void BeginFadeOut(Action callback)
        {
            _onFadeOutfinished = callback;
            _isFadingOut = true;
            _isFadingIn = false;
            _screenCover.gameObject.SetActive(true);
        }

        private void FinishFadingIn()
        {
            _onFadeInfinished?.Invoke();
            _isFadingIn = false;
            _screenCover.gameObject.SetActive(false);
        }

        private void FinishFadingOut()
        {
            _onFadeOutfinished?.Invoke();
            _isFadingOut = false;
        }

        private void BackToMainMenu()
        {
            _playerProfile.CurrentState.Value = GameState.Menu;
        }

        private void SetPause(bool isPaused)
        {
            _pauseView.gameObject.SetActive(isPaused);

            Time.timeScale = isPaused ? 0 : 1;
        }

        private void ProcessPauseInput()
        {
            _isPauseMenuOpened = !_isPauseMenuOpened;
            SetPause(_isPauseMenuOpened);
        }

        public void UpdateScore(int score)
        {
            _score.Value += score;
        }

        public void RegisterHealthAndLivesTracking(SubscriptionProperty<int> livesCount, SubscriptionProperty<float> healthPercentage)
        {
            livesCount.SubscribeOnChange(_topUIBarView.UpdateLives);
            healthPercentage.SubscribeOnChange(_topUIBarView.UpdateHealthBar);

            _topUIBarView.UpdateLives(livesCount.Value);
            _topUIBarView.UpdateHealthBar(healthPercentage.Value);

            _unsubsribeHealthTracking = () =>
            {
                livesCount.UnSubscribeOnChange(_topUIBarView.UpdateLives);
                healthPercentage.UnSubscribeOnChange(_topUIBarView.UpdateHealthBar);
            };
        }

        #endregion


        #region IDisposeable

        protected override void OnDispose()
        {
            _unsubsribeHealthTracking?.Invoke();
            Time.timeScale = 1;
            _inputController.EscapeInput -= ProcessPauseInput;
            _score.UnSubscribeOnChange(_topUIBarView.UpdateScore);
            base.OnDispose();
        }

        #endregion
    }
}