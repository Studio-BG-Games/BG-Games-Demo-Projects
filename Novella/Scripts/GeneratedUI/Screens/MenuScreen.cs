using System.Collections.Generic;
using Scripts.Managers;
using Scripts.Menu;
using Scripts.UISystem;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Scripts.Serializables.User;
using UnityEngine.UI;
using Scripts;

namespace GeneratedUI
{
    public class MenuScreen : WindowController
    {
#pragma warning disable 0649
        [SerializeField] private RectTransform actsParentContainer;
        [SerializeField] private List<GameObject> storyCardPrefab;
#pragma warning restore 0649
        [SerializeField] private InputHandler inputHandler;

        [SerializeField] private List<RectTransform> containers = new List<RectTransform>();
        [SerializeField] private GameObject leftArrow, rightArrow;
        [SerializeField] private GameObject[] ads = new GameObject[3];
        [SerializeField] private List<Sprite> ruCoverSprites = new List<Sprite>();
        [SerializeField] private List<Sprite> engCoverSprites = new List<Sprite>();

        [SerializeField] private float swipeSpeed = 1f;
        [SerializeField] private Ease easeType;
        [SerializeField] private int fakeStoriesCount = 7;
        [SerializeField] private TextMeshProUGUI cocktails;
        [SerializeField] private TextMeshProUGUI diamonds;
        [SerializeField] private TextMeshProUGUI elixirs;

        private GameManager _gameManager;
        private FirebaseManager _fireBaseManager;
        //private ReplayStoryWindow _replayStoryWindow;
        private int _currentActIndex;
        private float _distance;
        private bool _inSwipe;
        private string _currentSound;

        private void Start()
        {
            _gameManager = GameManager.Instance;
            if(_gameManager.userData.email != "")//проверка на дефолт структуры не подходит
            {                
                UpdateCurrencyBar();
            }


            _fireBaseManager = FirebaseManager.Instance;
            _fireBaseManager.OnUserDataLoaded += UpdateCurrencyBar;
            inputHandler.OnSwipeEvent += NextAct;

            if (PlayerPrefs.GetInt("Popup Windows") == 0)
                ads[0].SetActive(true);

            if (PlayerPrefs.GetInt("Popup Windows") == 1)
                ads[1].SetActive(true);

            if (PlayerPrefs.GetInt("Popup Windows") == 2)
                ads[2].SetActive(true);

            if (PlayerPrefs.GetInt("Popup Windows") < 3)
                PlayerPrefs.SetInt("Popup Windows", PlayerPrefs.GetInt("Popup Windows") + 1);
            

            //подписать еще на ивент покупки (доната) в магазине самой валюты. (траты с риплея истории, акта закомментировал)
            _currentSound = GameManager.Instance.soundManager.PlaySounds(new string[1] { "1. Меню, гардероб" }, 1.5f, 1.5f);

            if (PlayerPrefs.GetString("Volume") == "false")
            {
                GameManager.Instance.soundManager.Mute(true);
            }
            else
            {              
                GameManager.Instance.soundManager.Mute(false);
            }

            
        }

        private void OnDestroy()
        {
            inputHandler.OnSwipeEvent -= NextAct;
            //_gameManager.OnLoadUserDataComplete -= UpdateCurrencyBar;
            _fireBaseManager.OnUserDataLoaded -= UpdateCurrencyBar;
            
            // if(_replayStoryWindow != null)
            //     _replayStoryWindow.OnReplayComplete -= UpdateCurrencyBar;
        }

        // TEMP
        public void SignOut()
        {
            FirebaseManager.Instance.SignOut();
            WindowsManager.Instance.CreateScreen<RegisterScreen>();
        }

        public void PopulateStories()
        {
            /*var index = 0;
            GameManager.Instance.stories.ForEach(story =>
            {
                StoryCard storyCard = Instantiate(storyCardPrefab, actsParentContainer).GetComponent<StoryCard>();
                    storyCard.Initialize(story);
                    var storyCartRectTransform = storyCard.GetComponent<RectTransform>();
                    _distance = actsParentContainer.rect.width;
                    storyCartRectTransform.DOAnchorPosX(actsParentContainer.rect.width * index, 0);
                    containers.Add(storyCartRectTransform);
                    index++;
            });*/

            CreateFakeStory(fakeStoriesCount);
            UpdateButtonsView();
        }

        private void CreateFakeStory(int count)
        {
            for (int i = 0; i < count; i++)
            {
                GameManager.Instance.stories.ForEach(story =>
                {
                    story.id = i.ToString() ;
                    StoryCard storyCard = Instantiate(storyCardPrefab[i], actsParentContainer).GetComponent<StoryCard>();
                    storyCard.SetInputHandlerRef(inputHandler);
                    storyCard.Initialize(story);
                    var storyCartRectTransform = storyCard.GetComponent<RectTransform>();
                    var rect = actsParentContainer.rect;
                    _distance = rect.width;
                    storyCartRectTransform.anchoredPosition = new Vector2(rect.width * i, 0);
                    containers.Add(storyCartRectTransform);
                });
            }
        }

        public void OpenShopCurrencyWindow(int contentTypeIndex)
        {
            var shopWindow = WindowsManager.Instance.CreateWindow<ShopCurrencyWindow>();
            shopWindow.OpenNewContent(contentTypeIndex);
            OnWindowOpen(shopWindow);
        }

        public void OpenPackWindow()
        {
            var packWindow = WindowsManager.Instance.CreateWindow<PackWindow>();
            OnWindowOpen(packWindow);
        }

        public void OpenSettingsWindow()
        {
            var settingsWindow = WindowsManager.Instance.CreateWindow<SettingsWindow>();
            OnWindowOpen(settingsWindow);
        }

        private void OnWindowOpen(WindowController windowController)
        {
            inputHandler.enabled = false;
            windowController.onWindowClose += () => inputHandler.enabled = true;
        }

        public void NextAct(bool side)
        {
            var indexChanger = side ? 1 : -1;
            if (_inSwipe) return;
            if (!IndexCheck(indexChanger)) return;
            _inSwipe = true;
            foreach (var rectTransform1 in containers)
            {
                rectTransform1.DOAnchorPosX(rectTransform1.anchoredPosition.x - _distance * indexChanger, swipeSpeed)
                    .SetEase(easeType)
                    .OnComplete(() => _inSwipe = false);
            }

            _currentActIndex += indexChanger;
            UpdateButtonsView();
        }

        private bool IndexCheck(int indexChanger)
        {
            if (_currentActIndex == 0 && indexChanger == -1) return false;
            return _currentActIndex != containers.Count - 1 || indexChanger != 1;
        }

        private void UpdateButtonsView()
        {
            leftArrow.SetActive(_currentActIndex > 0);
            rightArrow.SetActive(_currentActIndex < containers.Count - 1);
        }

        public void UpdateCurrencyBar()
        {            
            cocktails.text =  _gameManager.userData.currencies.cocktails.ToString();
            diamonds.text = _gameManager.userData.currencies.cash.ToString();
            elixirs.text = _gameManager.userData.currencies.elixirs.ToString();
        }

        
        public void UpdateCurrencyBar(User user)
        {            
            Debug.Log("FireBaseManager.OnUserDataLoaded has been invoked");
            cocktails.text =  user.currencies.cocktails.ToString();
            diamonds.text = user.currencies.cash.ToString();
            elixirs.text = user.currencies.elixirs.ToString();
        }

        public void UpdateCoverSprite()
        {
            switch (PlayerPrefs.GetString("language"))
            {
                case "rus":
                    for (int i = 0; i < containers.Count; i++)
                    {
                        containers[i].GetComponent<StoryCover>().cover.sprite = ruCoverSprites[i];
                    }                 
                    break;
                case "eng":
                    for (int i = 0; i < containers.Count; i++)
                    {
                        containers[i].GetComponent<StoryCover>().cover.sprite = engCoverSprites[i];
                    }
                    break;
            }
        }
        // public void GetReplayStoryWindowInstance(ReplayStoryWindow instance)
        // {
        //     _replayStoryWindow = instance;
        //     _replayStoryWindow.OnReplayComplete += UpdateCurrencyBar;
        // }
    }
}