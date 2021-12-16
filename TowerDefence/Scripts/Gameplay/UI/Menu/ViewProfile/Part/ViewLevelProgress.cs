using DefaultNamespace.Infrastructure.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.UI.Menu.Canvas
{
    public class ViewLevelProgress : ViewPartProfile
    {
        [SerializeField] private Image _image;

        public override void View<T, TData>(ObjectCardProfile<T, TData> profileToView)
        {
            if(!(profileToView is LevelSetProfile))
                return;
            var p = profileToView as LevelSetProfile;
            _image.fillAmount = (float) (p.CurrentData.CurrentIndexLevel) / (p.Target.CountLevel);
        }
    }
}