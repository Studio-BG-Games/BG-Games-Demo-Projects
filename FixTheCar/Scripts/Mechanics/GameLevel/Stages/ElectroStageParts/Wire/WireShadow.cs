using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Mechanics.GameLevel.Stages.ElectroStageParts.Wire
{
    public class WireShadow : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private Color _colorLight;
        [SerializeField] private Color _colorDark;
        [SerializeField] private float _duration;

        private Tween _tween;

        private void Awake() => _image.color = _colorDark;

        public void Selected()
        {
            if(_tween!=null)
                return;
            _tween = _image.DOColor(_colorLight, _duration).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
        }

        public void Unselected()
        {
            if(_tween==null)
                return;
            _tween.Kill();
            _image.color = _colorDark;
            _tween = null;
        }

        [ContextMenu("TO DARK")]
        private void ToDark()
        {
            if (_image) _image.color = _colorDark;
        }
        
        [ContextMenu("TO LIGHT")]
        private void ToLight()
        {
            if (_image) _image.color = _colorLight;
        }
    }
}