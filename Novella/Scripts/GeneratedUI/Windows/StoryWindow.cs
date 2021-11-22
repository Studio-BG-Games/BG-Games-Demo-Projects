using System;
using Scripts.Managers;
using Scripts.UISystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Scripts;

namespace GeneratedUI
{
    public class StoryWindow : WindowController
    {
        [SerializeField] private TextMeshProUGUI chapter;
        [SerializeField] private TextMeshProUGUI localizedActDescription;

        private LoadingWindow _loadingWindow;

        // private ReplayStoryWindow _replayStoryWindow;
        // private BuyStoryActWindow _buyStoryActWindow;
        private WindowController _windowInstance;

        private void Start()
        {
            // TODO: Set Bacgkround;
            // TODO: Set Text;
            // TODO: Show Progress

            ref var user = ref GameManager.Instance.userData;
            var selectedStory = GameManager.Instance.selectedStory;
            var progress = user.progress.Find(value => value.storyId == selectedStory.id);
            string actIdDescriptionKey = "";
            if (Int32.TryParse(selectedStory.id, out var storyId))
            {
                actIdDescriptionKey = "act" + (storyId + 1) + "_desc";
            }

            var actDescription = selectedStory.general.Find(value => value.system == actIdDescriptionKey);
            switch (PlayerPrefs.GetString("language"))
            {
                case "rus": 
                    if(progress!=null && progress.actId == (selectedStory.acts.Count))
                    {
                        chapter.text = "Серия завершена!";
                        localizedActDescription.text = "";
                        break;
                    }

                    if (progress == null) chapter.text = "Серия 1";
                    else chapter.text = "Серия " + (progress.actId + 1).ToString();
                    localizedActDescription.text = actDescription.russian;
                    break;
                case "eng":
                    if(progress!=null && progress.actId == (selectedStory.acts.Count))
                    {
                        chapter.text = "Series completed!";
                        localizedActDescription.text = "";
                        break;
                    }

                    if (progress == null) chapter.text = "Series 1";
                    else chapter.text = "Series " + (progress.actId + 1).ToString();
                    localizedActDescription.text = actDescription.english;
                    break;
            }
        }

        public void TryOpenStory()
        {
            var user = GameManager.Instance.userData;
            var selectedStory = GameManager.Instance.selectedStory;
            var progress = user.progress.Find(value => value.storyId == selectedStory.id);
            if(progress!=null && progress.actId == (selectedStory.acts.Count))
            {
                return;
            }

            if (progress == null || progress.lastPurchasedAct < progress.actId) // < вместо != для переигрывания
            {
                WindowsManager.Instance.CreateWindow<BuyStoryActWindow>();
            }
            else
            {
                OpenStory();
            }
        }

        public void OpenStory()
        {
            _loadingWindow = WindowsManager.Instance.CreateWindow<LoadingWindow>();
            _loadingWindow.HideUnusedObjects();
            GameManager.Instance.effectsController.PlayFadeEffect(0.5f, null, () =>
            {
                WindowsManager.Instance.CreateScreen<StoryScreen>();
                CloseWindow();
            });
        }

        public void OpenWardrobe()
        {
            var user = GameManager.Instance.userData;
            var selectedStory = GameManager.Instance.selectedStory;
            var progress = user.progress.Find(value => value.storyId == selectedStory.id);
            if (progress == null) return;
            if (progress.IsHeroCreated)
                WindowsManager.Instance.CreateWindow<DressingRoomWindow>()
                    .ChooseWindowType(DressingRoomWindow.WindowType.Wardrobe);
        }

        public void OpenReplayStoryWindow()
        {
            var user = GameManager.Instance.userData;
            var selectedStory = GameManager.Instance.selectedStory;
            var progress = user.progress.Find(value => value.storyId == selectedStory.id);
            if (progress == null) return;
            if (progress.IsHeroCreated)
                WindowsManager.Instance.CreateWindow<ReplayStoryWindow>();
        }

        public void GetWindowInstance<T>(T instance) where T : WindowController
        {
            _windowInstance = instance;
            _windowInstance.onWindowCompleteDealEvent += TryOpenStory;
        }

        // public void GetBuyStoryActWindowInstance(BuyStoryActWindow instance)
        // {
        //     _buyStoryActWindow = instance;
        //     _buyStoryActWindow.OnBuyComplete += OpenStory;
        // }

        // public void GetReplayStoryWindowInstance(ReplayStoryWindow instance)
        // {
        //     _replayStoryWindow = instance;
        //     _replayStoryWindow.OnReplayComplete += OpenStory;
        // }

        private void OnDestroy()
        {
            // if(_buyStoryActWindow != null)
            //     _buyStoryActWindow.OnBuyComplete -= OpenStory;
            // if(_replayStoryWindow != null)
            //     _replayStoryWindow.OnReplayComplete -= OpenStory;
            if (_windowInstance != null)
                _windowInstance.onWindowCompleteDealEvent -= TryOpenStory;
        }
    }
}