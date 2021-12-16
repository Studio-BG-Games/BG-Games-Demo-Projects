using System.Linq;
using DefaultNamespace.Infrastructure.Data;
using Gameplay.HubObject.Data;
using Plugins.DIContainer;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.UI.Menu.Canvas
{
    public class ViewToggleInPull : ViewPartProfile
    {
        [SerializeField] private Image _image;
        [SerializeField] private Sprite _inPullSprite;
        [SerializeField] private Sprite _notInPullSprite;

        [DI] private DataGameMono _dataGameMono;
        
        public override void View<T, TData>(ObjectCardProfile<T, TData> profileToView)
        {
            if (profileToView is PlayerUnitProfile)
            {
                var p = profileToView as PlayerUnitProfile;
                var sprite = _dataGameMono.GetUnitPull().Contains(p.Target.MainDates.GetOrNull<IdContainer>().ID) ? _inPullSprite : _notInPullSprite;
                _image.sprite = sprite;
            }
            else if (profileToView is PlayerBuildProfile)
            {
                var p = profileToView as PlayerBuildProfile;
                var sprite = _dataGameMono.GetBuildPull().Contains(p.Target.MainDates.GetOrNull<IdContainer>().ID) ? _inPullSprite : _notInPullSprite;
                _image.sprite = sprite;
            }
            else
            {
                Debug.LogError($"у {profileToView} не может быть отображена галочка находится ли он в пуле или нет", this);
            }
        }
    }
}