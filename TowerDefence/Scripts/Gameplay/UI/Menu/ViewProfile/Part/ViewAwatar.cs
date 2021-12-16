using DefaultNamespace.Infrastructure.Data;
using DefaultNamespace.Infrastructure.Data.ParstOfCardProfiles;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.UI.Menu.Canvas
{
    public class ViewAwatar : ViewPartProfile
    {
        [SerializeField] private Image _image;
        [SerializeField] private bool _isBigAwatar;

        public override void View<T, TData>(ObjectCardProfile<T, TData> profileToView)
        {
            var avatar = profileToView.GetFirstOrNull<AvatarPart>();
            if(!avatar)
                return;
            Sprite sprite = _isBigAwatar ? avatar.Big : avatar.Mini;
            _image.sprite = sprite;
        }
    }
}