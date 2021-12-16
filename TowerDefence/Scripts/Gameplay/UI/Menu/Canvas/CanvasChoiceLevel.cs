using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.Infrastructure.Data;
using Infrastructure.ConfigData;
using Plugins.DIContainer;
using Plugins.HabObject;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Gameplay.UI.Menu.Canvas
{
    public class CanvasChoiceLevel : AbsMainMenuCanvas, IOpenViewProfilePanel
    {
        [Min(1)] [SerializeField] private int _countViewLevelOnOneScreen;
        [SerializeField] private HorizontalOrVerticalLayoutGroup _groupForViewLevel;
        [SerializeField] private Button _nextPanel;
        [SerializeField] private Button _prevPanel;
        [SerializeField] private Transform _rootChoiseLevel;
        
        [DI] private ConfigGame _configGame;
        [DI] private FactoryUIForMainMenu _factoryUi;
        [DI] private ContainerUIPrefabMainMenu _containerUiPrefab;

        private int _currentIndex = 0;
        private List<PackView> _packsView;
        private List<Transform> _rootViewLevel;

        protected override void CustomAwake()
        {
            _rootViewLevel = new List<Transform>();
            GeneratePackView();
            ViewPackByCurrentIndex();
            _nextPanel.onClick.AddListener(()=>ChangeIndexAt(1));
            _prevPanel.onClick.AddListener(()=>ChangeIndexAt(-1));
        }

        private void ChangeIndexAt(int changeAt)
        {
            _currentIndex += changeAt;
            if (_currentIndex < 0)
                _currentIndex = 0;
            if (_currentIndex >= _packsView.Count)
                _currentIndex = _packsView.Count - 1;
            ViewPackByCurrentIndex();
        }

        private void ViewPackByCurrentIndex()
        {
            var pack = _packsView[_currentIndex];
            RemoveChild(_groupForViewLevel);
            foreach (var levelSetProfile in pack.Profiles)
            {
                var view = _factoryUi.CreateViewProfile(_containerUiPrefab.ViewLevelSetProfileSmall);
                view.Init(levelSetProfile, this);
                view.transform.SetParent(_groupForViewLevel.transform);
            }
        }

        private void RemoveChild(HorizontalOrVerticalLayoutGroup groupForViewLevel)
        {
            foreach (var transform1 in groupForViewLevel.GetComponentsInChildren<Transform>().Except(new List<Transform>(){groupForViewLevel.transform}))
            {
                Destroy(transform1.gameObject);
            }
        }

        private void OnEnable()
        {
            _currentIndex = 0;
            ViewPackByCurrentIndex();
        }

        private void GeneratePackView()
        {
            var sets = _configGame.LevelsSets.OrderBy(x => x.Priority).TakeWhile(x=>true);
            _packsView = new List<PackView>();
            while (sets.Count() > 0)
            {
                int count = sets.Count() >= _countViewLevelOnOneScreen ? _countViewLevelOnOneScreen : sets.Count();
                var array = sets.Take(count);
                sets = sets.Except(array);
                LevelSetProfile[] profiles = new LevelSetProfile[count];
                int x = 0;
                foreach (var set in array)
                {
                    var profile = _configGame.LevelSetProfiles.First(x => x.Target.ID == set.ID);
                    profiles[x] = profile;
                    x++;
                }

                _packsView.Add(new PackView(profiles));
            }
        }

        private class PackView
        {
            public LevelSetProfile[] Profiles;

            public PackView(LevelSetProfile[] profiles) => Profiles = profiles;
        }

        public void Open<Card, T, TData>(ViewProfile<Card, T, TData> profile, Card card) where Card : ObjectCardProfile<T, TData> where T : Object where TData : SaveDataProfile
        {
            if (_rootViewLevel.Count >= 1)
            {
                var p = _factoryUi.CreateViewProfile(profile);
                
                _rootViewLevel[_rootViewLevel.Count-1].gameObject.SetActive(false);
                _rootViewLevel.Add(p.transform);
                _rootViewLevel[_rootViewLevel.Count-1].SetParent(RootPanel);
                ((RectTransform) _rootViewLevel[_rootViewLevel.Count-1].transform).anchoredPosition = Vector2.zero;
                ((RectTransform) _rootViewLevel[_rootViewLevel.Count-1].transform).sizeDelta = Vector2.zero;
                p.Init(card, this);
                _rootViewLevel[_rootViewLevel.Count-1].gameObject.SetActive(true);
                
            }
            else
            {
                var p = _factoryUi.CreateViewProfile(profile);
                
                _rootChoiseLevel.gameObject.SetActive(false);
                _rootViewLevel.Add(p.transform);
                _rootViewLevel[_rootViewLevel.Count-1].SetParent(RootPanel);
                ((RectTransform) _rootViewLevel[_rootViewLevel.Count-1].transform).anchoredPosition = Vector2.zero;
                ((RectTransform) _rootViewLevel[_rootViewLevel.Count-1].transform).sizeDelta = Vector2.zero;
                p.Init(card, this);
                _rootViewLevel[_rootViewLevel.Count-1].gameObject.SetActive(true);
            }
        }

        public void Close()
        {
            if (_rootViewLevel.Count > 1)
            {
                Destroy(_rootViewLevel[_rootViewLevel.Count-1].gameObject);
                _rootViewLevel.RemoveAt(_rootViewLevel.Count-1);
                _rootViewLevel[_rootViewLevel.Count-1].gameObject.SetActive(true);
                foreach (var reInitViewProfile in GetComponentsInChildren<IReInitViewProfile>()) reInitViewProfile.ReInit();
            }
            else
            {
                Destroy(_rootViewLevel[0].gameObject);
                _rootViewLevel.RemoveAt(0);
                _rootChoiseLevel.gameObject.SetActive(true);
                foreach (var reInitViewProfile in GetComponentsInChildren<IReInitViewProfile>()) reInitViewProfile.ReInit();
            }
        }
    }
}