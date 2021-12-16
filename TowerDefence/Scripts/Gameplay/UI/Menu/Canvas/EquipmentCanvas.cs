using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using DefaultNamespace.Infrastructure.Data;
using Gameplay.HubObject.Data;
using Infrastructure.ConfigData;
using Plugins.DIContainer;
using Plugins.HabObject;
using UnityEngine;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Object = UnityEngine.Object;

namespace Gameplay.UI.Menu.Canvas
{
    public class EquipmentCanvas : AbsMainMenuCanvas, IOpenViewProfilePanel
    {
        [DI] private ConfigGame _configGame;
        [DI] private FactoryUIForMainMenu _factoryUiForMainMenu;
        [DI] private ContainerUIPrefabMainMenu _containerUi;
        [DI] private DataGameMono _dataGameMono;

        [SerializeField] private Transform _rootChoiseSomeElement;
        [SerializeField] private Button _unitButton;
        [SerializeField] private Button _buildButton;
        [SerializeField] private RectTransform _viewParentBuyElement;
        [SerializeField] private RectTransform _viewParentElementInPull;
        [SerializeField] private ViewFullPullInNumber _viewFullPullIn;

        private Transform _rootProfile;
        private bool _buildIsSelected;
        private Action _updateViewPull;

        protected override void CustomAwake()
        {
            GenerateUnit();
            _unitButton.onClick.AddListener(()=>GenerateUnit());
            _buildButton.onClick.AddListener(()=>GenerateBuild());
        }

        public void Open<Card, T, TData>(ViewProfile<Card, T, TData> profile, Card card) where Card : ObjectCardProfile<T, TData> where T : Object where TData : SaveDataProfile
        {
            if(_rootProfile)
                return;
            var p = _factoryUiForMainMenu.CreateViewProfile(profile);
            _rootProfile = p.transform;
            _rootProfile.SetParent(RootPanel);
            ((RectTransform)_rootProfile.transform).anchoredPosition=Vector2.zero;
            ((RectTransform)_rootProfile.transform).sizeDelta=Vector2.zero;
            p.Init(card, this);
            _rootProfile.gameObject.SetActive(true);
            _rootChoiseSomeElement.gameObject.SetActive(false);
        }

        public void Close()
        {
            if(!_rootProfile) return;
            Destroy(_rootProfile.gameObject);
            _rootProfile = null;
            _rootChoiseSomeElement.gameObject.SetActive(true);
            foreach (var reInitViewProfile in GetComponentsInChildren<IReInitViewProfile>()) reInitViewProfile.ReInit();
            ClearChilds(_viewParentElementInPull);
            if(_buildIsSelected) DrawSmallBuild(); else DrawUnitSmall();
            _updateViewPull?.Invoke();
        }
        
        private void GenerateBuild()
        {
            UnsubscribeByChangeInPull();
            _dataGameMono.BuildPoolHasChahge += DrawSmallBuild;
            _buildIsSelected = true;
            var PlayerProfiles = _configGame.PlayerBuildProfiles;
            ClearChilds(_viewParentBuyElement);
            _viewFullPullIn?.View(PlayerProfiles[0]);
            _updateViewPull = () => _viewFullPullIn?.View(PlayerProfiles[0]);
            foreach (var playerProfile in PlayerProfiles)
            {
                var view = _factoryUiForMainMenu.CreateViewProfile(_containerUi.PlayerViewBuildBig);
                view.Init(playerProfile, this);
                view.transform.SetParent(_viewParentBuyElement);
            }
            DrawSmallBuild();
        }

        private void GenerateUnit()
        {
            UnsubscribeByChangeInPull();
            _dataGameMono.UnitPoolHasChahge += DrawUnitSmall;
            _buildIsSelected = false;
            var PlayerProfiles = _configGame.PlayerUnitProfiles;
            ClearChilds(_viewParentBuyElement);
            _viewFullPullIn?.View(PlayerProfiles[0]);
            _updateViewPull = () => _viewFullPullIn?.View(PlayerProfiles[0]);
            foreach (var playerProfile in PlayerProfiles)
            {
                var view = _factoryUiForMainMenu.CreateViewProfile(_containerUi.PleyerUnitViewBig);
                view.Init(playerProfile,this);
                view.transform.SetParent(_viewParentBuyElement);
            }

            DrawUnitSmall();
        }

        private void DrawSmallBuild()
        {
            ClearChilds(_viewParentElementInPull);
            var idsPull = _dataGameMono.GetBuildPull().ToList();
            var targetProfiles = _configGame.PlayerBuildProfiles
                .Where(x => idsPull.Contains(x.Target.MainDates.GetOrNull<IdContainer>().ID)).ToList();
            foreach (var profile in targetProfiles)
            {
                var view = _factoryUiForMainMenu.CreateViewProfile(_containerUi.PlayerViewBuildSmall);
                view.Init(profile, this);
                view.transform.SetParent(_viewParentElementInPull);
            }
        }

        private void DrawUnitSmall()
        {
            ClearChilds(_viewParentElementInPull);
            var idsPlayerInPull = _dataGameMono.GetUnitPull();
            foreach (var id in idsPlayerInPull)
            {
                var profile = _configGame.PlayerUnitProfiles.First(x => x.Target.MainDates.GetOrNull<IdContainer>().ID == id);
                if (!profile)
                {
                    Debug.LogWarning("No profile for this id - " + id);
                }

                var view = _factoryUiForMainMenu.CreateViewProfile(_containerUi.PleyerUnitViewSmall);
                view.Init(profile, this);
                view.transform.SetParent(_viewParentElementInPull);
            }
        }

        private void ClearChilds(RectTransform parent)
        {
            var childs = parent.GetComponentsInChildren<RectTransform>().Except(new List<RectTransform>() {parent});
            foreach (var rectTransform in childs) Destroy(rectTransform.gameObject);
        }

        private void UnsubscribeByChangeInPull()
        {
            _dataGameMono.BuildPoolHasChahge -= DrawSmallBuild;
            _dataGameMono.UnitPoolHasChahge -= DrawUnitSmall;
        }
    }
}