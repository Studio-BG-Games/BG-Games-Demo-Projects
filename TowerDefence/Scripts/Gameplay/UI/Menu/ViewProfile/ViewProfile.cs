using System;
using DefaultNamespace.Infrastructure.Data;
using Plugins.HabObject;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Gameplay.UI.Menu.Canvas
{
    public abstract class ViewProfile<Card, T, TData> : MonoBehaviour, IReInitViewProfile where Card : ObjectCardProfile<T, TData> where T : Object where TData : SaveDataProfile
    {
        private Card _profile;
        private ViewPartProfile[] _parts;

        public IOpenViewProfilePanel ProfilePanel => _profilePanel;
        private IOpenViewProfilePanel _profilePanel;


        private void Awake()
        {
            _parts = GetComponents<ViewPartProfile>();
        }

        public void Init(Card card, IOpenViewProfilePanel profilePanel)
        {
            _profile = card;
            _profilePanel = profilePanel;
            foreach (var viewPartProfile in _parts)
            {
                (viewPartProfile as IInitByIOpenViewProfilePanel)?.Init(_profilePanel);
                viewPartProfile.View(card);
            }
        }

        public void ReInit()
        {
            if(_profile)
                Init(_profile, _profilePanel);
        }
    }

    public interface IReInitViewProfile
    {
        void ReInit();
    }
}