using DefaultNamespace.Infrastructure.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.UI.Menu.Canvas
{
    public class ViewFinishLevelProfile : ViewPartProfile
    {
        [SerializeField] private Image _image;
        [SerializeField] private Sprite _mark;
        
        public override void View<T, TData>(ObjectCardProfile<T, TData> profileToView)
        {
            var p = profileToView as LevelSetProfile;
            _image.sprite = p.CurrentData.IsCompletedSet ? _mark : null;
        }
    }
}