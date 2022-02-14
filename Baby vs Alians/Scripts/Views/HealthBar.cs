using System.Collections;
using UnityEngine;

namespace Baby_vs_Aliens
{
    public class HealthBar : MonoBehaviour
    {
        #region Fields

        [SerializeField] SpriteRenderer _background;
        [SerializeField] SpriteRenderer _healthBar;

        private float _defaultBarSize;
        private Color _defaultColor;
        private Camera _camera;

        private IEnumerator _hideCoroutine;
        private float _hideTimer;

        #endregion


        #region UnityMethods

        private void Awake()
        {
            _defaultBarSize = _healthBar.size.x;
            _defaultColor = _healthBar.color;
            _camera = Camera.main;
        }

        private void LateUpdate()
        {
            transform.rotation = _camera.transform.rotation;
        }

        #endregion


        #region Methods

        public void SetBarSize(float percentage)
        {
            ShowHealthBar(true);

            var newBarSize = _defaultBarSize * percentage >= 0 ? _defaultBarSize * percentage : 0;

            _healthBar.size = new Vector2(newBarSize, _healthBar.size.y);
            if (percentage < 0.5f)
                _healthBar.color = Color.red;
            else
                _healthBar.color = _defaultColor;
        }

        public void SetToDisable(float time)
        {
            if (_hideCoroutine != null)
                StopCoroutine(_hideCoroutine);

            _hideTimer = time;

            _hideCoroutine = HideHealthBar();
            StartCoroutine(_hideCoroutine);
        }

        public void Disable()
        {
            ShowHealthBar(false);
        }

        private IEnumerator HideHealthBar()
        {
            yield return new WaitForSeconds(_hideTimer);
            ShowHealthBar(false);

            _hideCoroutine = null;
        }

        private void ShowHealthBar(bool isActive)
        {

            _healthBar.gameObject.SetActive(isActive);
            _background.gameObject.SetActive(isActive);
        }

        #endregion
    }
}