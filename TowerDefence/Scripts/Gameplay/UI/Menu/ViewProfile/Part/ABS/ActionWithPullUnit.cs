using System;
using System.Linq;
using DefaultNamespace.Infrastructure.Data;
using DefaultNamespace.Infrastructure.Data.ParstOfCardProfiles;
using Gameplay.HubObject.Data;
using Infrastructure.ConfigData;
using Plugins.DIContainer;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Gameplay.UI.Menu.Canvas
{
    public abstract class ActionWithPullUnit : ViewPartProfile
    {
        [SerializeField] private Button _button;
        
        [SerializeField] private UnityEvent ActionIfInPull;
        [SerializeField] private UnityEvent ActionIfNotInPull;
        [SerializeField] private UnityEvent Clicked;
        [SerializeField] private UnityEvent FailLoad;
        
        [DI] private ConfigGame _configGame;
        [DI] protected DataGameMono _dataGameMono;

        private IReInitViewProfile _initViewProfile;
        private UnityAction _unityAction;

        private void Awake()
        {
            _initViewProfile = GetComponent<IReInitViewProfile>();
        }

        public override void View<T, TData>(ObjectCardProfile<T, TData> profileToView)
        {
            if (!(profileToView is PlayerUnitProfile))
            {
                FailLoad?.Invoke();
                return;
            }
            var d = profileToView as PlayerUnitProfile;
            if (!d.CurrentData._isOpen)
            {
                FailLoad?.Invoke();
                return;
            }
            var p = profileToView as PlayerUnitProfile;
            _unityAction = ()=>
            {
                Action(p, p.Target.MainDates.GetOrNull<IdContainer>().ID);
                _initViewProfile?.ReInit();
                Clicked?.Invoke();
            };
            
            Init(p);
        }

        private void Init(PlayerUnitProfile profileToView)
        {
            _button.onClick.RemoveAllListeners();
            _button.onClick.AddListener(_unityAction);
            if (InPool(profileToView, out var pull))
            {
                ActionIfInPull?.Invoke();
            }
            else
            {
                ActionIfNotInPull?.Invoke();
            }
        }

        private bool InPool(PlayerUnitProfile profileToView, out string pullPlayerId)
        {
            pullPlayerId = profileToView.Target.MainDates.GetOrNull<IdContainer>().ID;
            return _dataGameMono.GetUnitPull().ToList().Contains(pullPlayerId);
        }

        protected abstract void Action(PlayerUnitProfile playerUnitProfile, string id);
    }
}