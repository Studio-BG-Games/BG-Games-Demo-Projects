using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.Infrastructure.Data.ParstOfCardProfiles;
using Gameplay.Builds;
using Gameplay.HubObject.Data;
using Infrastructure.ConfigData;
using Plugins.DIContainer;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.UI
{
    public class ViewAvatarOnConstructButton : PartInitConstructButton
    {
        private static Dictionary<string, Sprite> IdAndSprite = new Dictionary<string, Sprite>();

        [SerializeField] private Image _image;
        [SerializeField] private bool _isBigAvatar;

        [DI] private ConfigGame _configGame;
        
        public override void Init<T>(T template) 
            => _image.sprite = GetSprite(template.MainDates.GetOrNull<IdContainer>().ID, template is Build);

        private Sprite GetSprite(string id, bool isBuild)
        {
            if (IdAndSprite.ContainsKey(id))
                return IdAndSprite[id];

            if (isBuild)
            {
                var profile = _configGame.PlayerBuildProfiles.FirstOrDefault(x => x.Target.MainDates.GetOrNull<IdContainer>().ID == id);
                if (!profile)
                {
                    IdAndSprite.Add(id, null);
                    return null;
                }

                var avatar = profile.GetFirstOrNull<AvatarPart>();
                if (!avatar)
                {
                    IdAndSprite.Add(id, null);
                    return null;
                }
                
                IdAndSprite.Add(id, _isBigAvatar ? avatar.Big : avatar.Mini);
                return IdAndSprite[id];
            }
            else
            {
                var profile = _configGame.PlayerUnitProfiles.FirstOrDefault(x => x.Target.MainDates.GetOrNull<IdContainer>().ID == id);
                if (!profile)
                {
                    IdAndSprite.Add(id, null);
                    return null;
                }

                var avatar = profile.GetFirstOrNull<AvatarPart>();
                if (!avatar)
                {
                    IdAndSprite.Add(id, null);
                    return null;
                }
                
                IdAndSprite.Add(id, _isBigAvatar ? avatar.Big : avatar.Mini);
                return IdAndSprite[id];
            }
        }
    }
}