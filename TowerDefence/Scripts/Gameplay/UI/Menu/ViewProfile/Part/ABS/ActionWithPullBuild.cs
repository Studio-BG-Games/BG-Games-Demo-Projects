using System.Linq;
using DefaultNamespace.Infrastructure.Data;
using Gameplay.HubObject.Data;
using Infrastructure.ConfigData;
using Plugins.DIContainer;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Gameplay.UI.Menu.Canvas
{
    public abstract class ActionWithPullBuild : ViewPartProfile
    {
        [SerializeField] private UnityEvent ActionIfNotInPull;
        [SerializeField] private Button _button;
        [SerializeField] private UnityEvent ActionIfInPull;
        [SerializeField] private UnityEvent Clicked;
        
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
            if(!(profileToView is PlayerBuildProfile))
                return;
            var d = profileToView as PlayerBuildProfile;
            if(!d.CurrentData._isOpen)
                return;
            _unityAction = ()=>
            {
                Action(profileToView as PlayerBuildProfile);
                Clicked?.Invoke();
                _initViewProfile?.ReInit();
            };
            Init(profileToView as PlayerBuildProfile);
        }

        private void Init(PlayerBuildProfile profileToView)
        {
            _button.onClick.RemoveAllListeners();
            _button.onClick.AddListener(_unityAction);
            if (InPool(profileToView))
            {
                ActionIfInPull?.Invoke();
            }
            else
            {
                ActionIfNotInPull?.Invoke();
            }
        }

        private bool InPool(PlayerBuildProfile profileToView)
        {
            var id = profileToView.Target.MainDates.GetOrNull<IdContainer>().ID;
            return _dataGameMono.GetBuildPull().ToList().Contains(id);
        }

        protected abstract void Action(PlayerBuildProfile profileToView);

    }
}