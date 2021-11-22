using DefaultNamespace;
using Infrastructure.Configs;
using Plugins.DIContainer;
using UnityEngine;
using UnityEngine.UI;

namespace Mechanics.SketchBook
{
    public class Sticker : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private Color _offColor;
        [SerializeField] private Color _onColor;
        
        [DI] private DataFinishedLevel _dataFinishedLevel;
        
        public void Init(ConfigLevel level)
        {
            _image.color = _dataFinishedLevel.GetAllFinishedLevel().Contains(level.name) ? _onColor : _offColor;
            _image.sprite = level.Stiker;
        }
    }
}