using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Scripts.Managers;
using Scripts.Serializables.Story;
using Scripts.Serializables.User;
using Scripts.UISystem;
using MiniJSON;
using UnityEngine;
using UnityEngine.UI;
using Scripts;

namespace GeneratedUI
{
    public class StoryScreen : WindowController
    {
        [SerializeField] private DialogWindow dialogWindow;
        [SerializeField] private OptionWindow optionWindow;
        [SerializeField] private BackgroundUpdater backgroundUpdater;
        [SerializeField] private CharacterContainer characterContainer;
        [SerializeField] private float nextBackgroundTransitionTime = 1f;
        [SerializeField] private GameObject subMenuButtons, soundOn, soundOff;
        [SerializeField] private BackgroundAnimations backgroundAnimations;

        [SerializeField] private int _screensCountAds = 150;

        private Act _act;
        private Story _story;
        private Record _currentRecord;
        private int _currentRecordIndex;
        public StoryProgress _storyProgress;
        public User _user; // was private
        private DebugWindow _debugWindow;
        private DressingRoomWindow _characterCreationWindow;
        private EnterPlayerNameWindow _enterPlayerNameWindow;

        private bool _isFirstScreenUpdate = true;
        private bool _isChainRecord;
        private bool _isNextFade = false;
        private int _savedRecordIndex;
        private string _selectedChainIndex;

        private string _currentBackground;
        private string _currentNpc;
        private string _currentNpcEmotion;
        private string _lastNpcEmotion;
        private string _currentSound;
        private string _lastNpc;

        private PlayerInformation _playerInformation => GameManager.Instance.playerInformation;


        private int _currentScreensCount = 0;

        private void Start()
        {
            _currentScreensCount = PlayerPrefs.GetInt("ScreensCount", 0);
            _story = GameManager.Instance.selectedStory;
            _user = GameManager.Instance.userData;
            // Get user progress for this story.
            _storyProgress = _user.progress.FirstOrDefault(p => p.storyId == _story.id) ?? CreateNewStoryProgress();
            // Get corresponding act and record index
            _act = _story.acts.Find(a => a.id == _storyProgress.actId);
            _currentRecordIndex = _storyProgress.recordId;
            // Display Current Record
            GoToNextScreen();
            SubscribeToEvents();
            SoundCheck();

#if UNITY_EDITOR
            if (_debugWindow != null) return;
            _debugWindow = WindowsManager.Instance.CreateWindow<DebugWindow>();
            _debugWindow.SetStoryScreenReference(this);
#endif
        }

        private StoryProgress CreateNewStoryProgress()
        {
            var storyProgress = new StoryProgress(_story.id);
            _user.progress.Add(storyProgress);
            return storyProgress;
        }

        private void OnApplicationQuit()
        {
            SaveProgress();
        }

        private void SaveProgress()
        {
            _storyProgress.recordId = _currentRecordIndex;
            _storyProgress.actId = _act.id;
            _user.currencies = GameManager.Instance.userData.currencies;
            GameManager.Instance.userData = _user;
            FirebaseManager.Instance.UpdateUserData();
            //ES3.Save("user", _user);
        }

        private void SubscribeToEvents()
        {
            dialogWindow.OnDialogButtonClick += GoTexNextScreenSetup;
            optionWindow.OnOptionSelected += SelectOption;
        }

        private void UnsubscribeFromEvents()
        {
            optionWindow.OnOptionSelected -= SelectOption;
            dialogWindow.OnDialogButtonClick -= GoTexNextScreenSetup;
        }

        private void OnDestroy()
        {
            PlayerPrefs.SetInt("ScreensCount", _currentScreensCount);
            UnsubscribeFromEvents();
            SaveProgress();
        }

        private bool IsLastRecord()
        {
            if (_currentRecordIndex < _act.records.Count) return false;
            GameManager.Instance.effectsController.PlayFadeEffect(0.5f, null,
                () =>
                {
                    WindowsManager.Instance.CreateWindow<GameOverWindow>();
                    WindowsManager.Instance.CreateWindow<GameOverWindow>()?.OutputOfResults(_storyProgress);
                });
            _act.id++;

            _currentRecordIndex = 0;
            _storyProgress.isQLinesFromPreviousActAdded = false;
            //if(_storyProgress.reputation.ContainsKey("reputation"))// Данный код добавлен в GameOverWindow т.к. инумератор вызывается позже
            //    _storyProgress.globalReputationBeforeAct = _storyProgress.reputation["reputation"]; 
            _storyProgress.lastActReputation = new Dictionary<string, int>(_storyProgress.reputation);
            _storyProgress.isElixirOn = false;
            _storyProgress.heroDressKeyBeforeAct = _storyProgress.heroDressKey;
            _storyProgress.heroHairKeyBeforeAct = _storyProgress.heroHairKey;
            var selectedStory = GameManager.Instance.selectedStory;
            if (Int32.TryParse(selectedStory.id, out var storyId))
            {
                var story_progress = GameManager.Instance.userData.progress[storyId];
                story_progress.qlinesActLast = new List<string>(story_progress.qlinesActCurrent);
                story_progress.qlinesActCurrent = new List<string>();
                GameManager.Instance.userData = _user;
                FirebaseManager.Instance.UpdateUserData();
                //ES3.Save("user", _user);
            }

            // if(_act.id >= 6) // TO DELETE
            // {
            //     //_storyProgress = null;
            //     GameManager.Instance.userData.progress[storyId] = new StoryProgress(selectedStory.id);
            //     FirebaseManager.Instance.UpdateUserData();
            //     //ES3.Save("user", _user);
            // }
            return true;
        }

        private void SetPlayerName(string userName)
        {
            dialogWindow.gameObject.SetActive(true);
            _playerInformation.playerName = userName;
            GoTexNextScreenSetup();
        }

        #region methods for debug window only

        public void ChangeDialogIndex(int index)
        {
            _currentRecordIndex = index;
            GoTexNextScreenSetup();
        }

        public int GetDialogIndex()
        {
            var dialogIndex = _currentRecordIndex;
            return dialogIndex;
        }

        public int GetActIndex()
        {
            var actIndex = _storyProgress.actId;
            return actIndex;
        }

        public void ChangeActIndex(int index)
        {
            _act = _story.acts.Find(a => a.id == index);
            _currentRecordIndex = 0;
            _storyProgress.isQLinesFromPreviousActAdded = false;
            _storyProgress.isElixirOn = false;
            var selectedStory = GameManager.Instance.selectedStory;
            if (Int32.TryParse(selectedStory.id, out var storyId))
            {
                if (index != _storyProgress.actId)
                {
                    var story_progress = GameManager.Instance.userData.progress[storyId];
                    story_progress.qlinesActLast = new List<string>(story_progress.qlinesActCurrent);
                    //story_progress.qlinesActCurrent.Clear();
                    story_progress.qlinesActCurrent = new List<string>();
                    while (story_progress.qlines.Count <= index) story_progress.qlines.Add(new List<string>());
                    story_progress.qlines[index] = new List<string>(story_progress.qlinesActLast);

                    story_progress.lastActReputation = new Dictionary<string, int>(story_progress.reputation);
                    if (!story_progress.reputation.ContainsKey("reputation"))
                    {
                        story_progress.reputation.Add("reputation", 0);
                    }

                    story_progress.globalReputationBeforeAct = story_progress.reputation["reputation"];
                    story_progress.heroDressKeyBeforeAct = story_progress.heroDressKey;
                    story_progress.heroHairKeyBeforeAct = story_progress.heroHairKey;
                    // _storyProgress.actId = _act.id;
                }
                else
                    _storyProgress.isQLinesFromPreviousActAdded = true;
            }

            _storyProgress.lastPurchasedAct = index;
            GameManager.Instance.userData = _user;
            FirebaseManager.Instance.UpdateUserData();
            //ES3.Save("user", _user);
            GoTexNextScreenSetup();
        }

        #endregion


        private void GoTexNextScreenSetup()
        {
            _currentRecordIndex++;
            GoToNextScreen();
        }

        private void GoToNextScreen()
        {
            dialogWindow.SetDialogButtonState(true);

            Debug.Log($"{_currentRecordIndex}/{_act.records.Count}");
            if (IsLastRecord()) return;
            if (_isChainRecord && _currentRecord.type != "chain") ChainProgress();

            _currentRecord = _act.records[_currentRecordIndex];


            string debug = $"INDEX {_currentRecordIndex} | TYPE {_currentRecord.type}" +
                           (_currentRecord.qline != null && _currentRecord.qline.Length > 0
                               ? (" | QLINE " + _currentRecord.qline[0])
                               : "") + $" | TEXT {_currentRecord.text}";

            Debug.Log(debug);

            if (!_storyProgress.CanReadRecord(_currentRecord))
            {
                Debug.Log("Cant read record" + _currentRecord.row);
                GoTexNextScreenSetup();
                return;
            }

            var nextBackground = _currentRecord.background;

            if (_currentBackground == nextBackground || _isFirstScreenUpdate)
            {
                _isFirstScreenUpdate = false;
                UpdateScreenWithoutFadeEffect();
            }
            else UpdateScreenWithFadeEffect();

            if (_isNextFade)
            {
                _isNextFade = false;
                UpdateScreenWithFadeEffect();
            }


            _currentBackground = nextBackground;
            PlayEffects();

            Debug.Log("Interstitial available: " + SDKManager.Instance.InterstitialAvailable);
            if (_currentScreensCount++ >= _screensCountAds && SDKManager.Instance.InterstitialAvailable)
            {
                //if IAP bought - no ads
                if (PlayerPrefs.GetInt("NO_ADS", 0) == 1) return;
                
                var window = WindowsManager.Instance.CreateWindow<AdWindow>();
                window.SetCloseCallback(() => { SDKManager.Instance.ShowInterstitial(null); });
                _currentScreensCount = 0;
                ControlTouchPanel(false);
            }
        }

        private void UpdateScreenWithoutFadeEffect()
        {
            backgroundUpdater.UpdateBackgroundSprite(_currentRecord);
            backgroundAnimations.UpdateAnimation(_currentRecord.background, _currentRecord.backgroundOptions);
            UpdateSoundSetup();

            switch (PlayerPrefs.GetString("language"))
            {
                case "rus":
                    UpdateTextSetup(_currentRecord.text);
                    break;
                case "eng":
                    UpdateTextSetup(_currentRecord.english);
                    break;
            }

            UpdateCharacterSetup(_currentRecord.npc, false);
            _storyProgress.qlinesActCurrent = _storyProgress.actQlines;

            RecordTypeCheck();
        }

        private void UpdateScreenWithFadeEffect()
        {
            GameManager.Instance.effectsController.PlayFadeEffect(nextBackgroundTransitionTime,
                () =>
                {
                    characterContainer.HideAllCharacters();
                    dialogWindow.gameObject.SetActive(false);
                    UpdateSoundSetup();
                },
                () =>
                {
                    backgroundUpdater.UpdateBackgroundSprite(_currentRecord);
                    backgroundAnimations.UpdateAnimation(_currentRecord.background, _currentRecord.backgroundOptions);
                    RecordTypeCheck();
                }, () =>
                {
                    UpdateCharacterSetup(_currentRecord.npc, true);
                    //TODO: Discuss tranlations
                    switch (PlayerPrefs.GetString("language"))
                    {
                        case "rus":
                            UpdateTextSetup(_currentRecord.text);
                            break;
                        case "eng":
                            UpdateTextSetup(_currentRecord.english);
                            break;
                    }
                });
        }

        private void RecordTypeCheck()
        {
            switch (_currentRecord.type)
            {
                case "personalize":
                    ShowPersonalizeWindow();
                    break;
                case "question":
                    ShowQuestionWindow();
                    break;
                case "chain":
                    ShowQuestionChainWindow();
                    break;
                case "hquestion":
                    ShowHQuestionWindow();
                    break;
                case "setquestion":
                    ShowSetQuestionWindow();
                    break;
                case "name":
                    ShowEnterPlayerNameScreen();
                    break;
            }
        }

        private void ShowSetQuestionWindow()
        {
            string variableForShop = null;

            if (_currentRecord.npcOptions != null && _currentRecord.npcOptions[0] == "set:partydress")
                variableForShop = "partydress";

            _savedRecordIndex = _currentRecordIndex;
            dialogWindow.SetDialogButtonState(false);

            var shopWindow = WindowsManager.Instance.CreateWindow<DressingRoomWindow>();
            shopWindow.OpenShop(_currentRecord.options, variableForShop);
            //shopWindow.OnShopComplete += SelectOption;
            shopWindow.OnShopCompleteBestItem += SelectShopOption;
        }

        private void PlayEffects()
        {
            if (_currentRecord.backgroundOptions == null) return;
            foreach (var backgroundOption in _currentRecord.backgroundOptions)
            {
                if (backgroundOption == "shake")
                {
                    backgroundUpdater.DoBackgroundShake();
                }

                if (backgroundOption == "faded")
                {
                    _isNextFade = true;
                }

                if (backgroundOption == "poster")
                {
                    backgroundUpdater.DoBackgroundScroll(
                        _currentRecord,
                        () => dialogWindow.SetDialogButtonState(false),
                        () => dialogWindow.SetDialogButtonState(true));
                }
            }
        }

        private void UpdateCharacterSetup(string newNpc, bool isAfterFade)
        {
            _lastNpc = _currentNpc;
            _currentNpc = newNpc;
            _lastNpcEmotion = _currentNpcEmotion;
            _currentNpcEmotion = _currentRecord.emotion;

            dialogWindow.SetDialogButtonState(false);

            if (_lastNpc != _currentNpc || (_lastNpc == _currentNpc && isAfterFade))
            {
                if (_currentNpc == "protagonist")
                {
                    characterContainer.ShowHero(_currentRecord);
                }
                else if (_currentNpc != null)
                {
                    characterContainer.ShowNpc(_currentRecord);
                }
                else if (_currentNpc == null)
                {
                    characterContainer.HideAllCharacters();
                }

                backgroundUpdater.UpdateSide(_currentNpc);
            }

            if (_lastNpcEmotion != _currentNpcEmotion ||
                (_lastNpcEmotion == _currentNpcEmotion && _lastNpc != _currentNpc))
            {
                if (_currentNpc == "protagonist")
                {
                    characterContainer.ChangeEmotionsHero(_currentRecord, _lastNpc, _currentNpc, dialogWindow);
                }
                else if (_currentNpc != null)
                {
                    characterContainer.ChangeEmotionsNpc(_currentRecord, _lastNpc, _currentNpc, dialogWindow);
                }
                else
                {
                    characterContainer.TurnOnDialog(dialogWindow, _currentRecord);
                }
            }
            else
            {
                characterContainer.TurnOnDialog(dialogWindow, _currentRecord);
            }
        }

        private void UpdateSoundSetup()
        {
            if (_currentRecord.sound == null)
            {
                if (_currentRecord.type == "setquestion" || _currentRecord.type == "hquestion")
                {
                    _currentSound = GameManager.Instance.soundManager.PlaySounds(new string[1] {"1. Меню, гардероб"},
                        nextBackgroundTransitionTime, nextBackgroundTransitionTime);
                    return;
                }

                if (GameManager.Instance.soundManager.CheckAudioSource())
                    GameManager.Instance.soundManager.StopCurrentSound(2.5f);
                return;
            }

            if (_currentSound != _currentRecord.sound[0])
            {
                StartCoroutine(ChangeSound());
            }
        }

        private void UpdateTextSetup(string text)
        {
            if (_currentRecord.text == null) return;
            dialogWindow.gameObject.SetActive(true);
            //string npcLocalizedName;
            //var test = GameManager.Instance; В ожидании парсинга general таблицы
            switch (text)
            {
                case "rus":
                    //npcLocalizedName = _currentRecord.npc                                   
                    break;
                case "eng":
                    break;
            }

            dialogWindow.UpdateText(text, _currentRecord.npc, _playerInformation.playerName, _currentRecord.npcOptions);
        }

        private void ShowHQuestionWindow()
        {
            dialogWindow.SetDialogButtonState(false);
            dialogWindow.gameObject.SetActive(false);
            var backgroundShop = WindowsManager.Instance.CreateWindow<BackgroundShopWindow>();
            backgroundShop.InitItems(_currentRecord.options);
            backgroundShop.OnBackgroundSelected += SelectOption;
        }

        private void ChainProgress()
        {
            if (_act.records[_currentRecordIndex].qline != null &&
                _storyProgress.GetCurrentChainIndex(_currentRecord) == _selectedChainIndex) return;
            if (!optionWindow.ChainButtonCompeteCheck())
            {
                _currentRecordIndex = _savedRecordIndex;
                _storyProgress.CleanQline();
            }
            else
            {
                optionWindow.DeleteAllChainButtons();
                _isChainRecord = false;
            }
        }

        private void ShowEnterPlayerNameScreen()
        {
            dialogWindow.SetDialogButtonState(false);
            dialogWindow.gameObject.SetActive(false);
            _enterPlayerNameWindow = WindowsManager.Instance.CreateWindow<EnterPlayerNameWindow>();
            _enterPlayerNameWindow.OnComplete += SetPlayerName;
        }

        public void ControlTouchPanel(bool state)
        {
            dialogWindow.SetDialogButtonState(state);
            dialogWindow.gameObject.SetActive(state);
            if (!state)
            {
                if (_currentNpc != null)
                {
                    characterContainer.HideAllCharacters();
                }
            }
            else UpdateCharacterSetup(_currentNpc, true);
        }

        private void ShowPersonalizeWindow()
        {
            dialogWindow.gameObject.SetActive(false);
            _characterCreationWindow = WindowsManager.Instance.CreateWindow<DressingRoomWindow>();
            _characterCreationWindow.ChooseWindowType(DressingRoomWindow.WindowType.Personalize);
            _characterCreationWindow.OnCharacterCustomizationComplete += GoTexNextScreenSetup;
        }

        private void ShowQuestionWindow()
        {
            dialogWindow.SetDialogButtonState(false);
            dialogWindow.ShowOptionsWindow();
            if (TimerCheck())
            {
                var duration = GetOptionTimer();
                optionWindow.CreateTimer(duration);
            }

            var manyOptions = false;
            if (_currentRecord.options.Length > 3)
            {
                manyOptions = true;
                optionWindow.GetComponent<VerticalLayoutGroup>().spacing = 0;
            }
            else optionWindow.GetComponent<VerticalLayoutGroup>().spacing = 40;

            // TODO: Display Window with text
            for (int i = 0; i < _currentRecord.options.Length; i++)
            {
                Debug.Log($"KEY {i + 1}, Option {i}: {_currentRecord.options[i].text}");
                // TODO: Spawn Buttons For each answer in this window.
                var id = i;
                if (manyOptions) optionWindow.GetComponent<VerticalLayoutGroup>().spacing = -28;
                else optionWindow.GetComponent<VerticalLayoutGroup>().spacing = 40;
                optionWindow.CreateOptionButton(_currentRecord.options[id], id, manyOptions);
            }
        }

        private float GetOptionTimer()
        {
            float timer = 0;
            foreach (var option in _currentRecord.options)
            {
                if (option.timer > 0)
                {
                    timer = option.timer;
                }
            }

            return timer;
        }

        private bool TimerCheck()
        {
            foreach (var option in _currentRecord.options)
            {
                if (option.timer > 0)
                {
                    return true;
                }
            }

            return false;
        }

        private void ShowQuestionChainWindow()
        {
            dialogWindow.SetDialogButtonState(false);
            if (!_isChainRecord)
            {
                for (int i = 0; i < _currentRecord.options.Length; i++)
                {
                    Debug.Log($"KEY {i + 1}, Option {i}: {_currentRecord.options[i].text}");
                    // TODO: Spawn Buttons For each answer in this window.
                    var id = i;
                    var chainButton = optionWindow.CreateChainButton();
                    chainButton.button.onClick.AddListener(() =>
                    {
                        dialogWindow.HideOptionsWindow();
                        SelectChainOption(id);
                    });

                    switch (PlayerPrefs.GetString("language"))
                    {
                        case "rus":
                            chainButton.chainDescription.SetText(_currentRecord.options[id].text);
                            break;
                        case "eng":
                            chainButton.chainDescription.SetText(_currentRecord.options[id].english);
                            break;
                    }
                }

                _savedRecordIndex = _currentRecordIndex;
                _isChainRecord = true;
            }

            dialogWindow.ShowOptionsWindow();
        }

        /// <summary>
        /// Helper Methods
        /// </summary>
        private void SelectOption(int i)
        {
            Debug.LogWarning("Selecting Option: " + i);
            if (!_storyProgress.SelectOption(i, _currentRecord)) return;
            Debug.Log("RECORD INDEX " + _currentRecordIndex);

            if (_currentRecord.options[i].effect == "reputation" && _currentRecord.options[i].points != 0)
            {
                WindowsManager.Instance.CreateWindow<ReputationWindow>();
                WindowsManager.Instance.CreateWindow<ReputationWindow>()?.SetText(_currentRecord.options[i].points);
            }
            else if (_currentRecord.options[i].effect != null && _currentRecord.options[i].effect != "reputation")
            {
                WindowsManager.Instance.CreateWindow<RelationsWindow>();
                WindowsManager.Instance.CreateWindow<RelationsWindow>()?.SetText(_currentRecord.options[i].effect,
                    _currentRecord.options[i].points);
            }

            GoTexNextScreenSetup();
        }

        private void SelectShopOption(int i, int points)
        {
            Debug.LogWarning("Selecting Option: " + i);
            if (!_storyProgress.SelectOption(i, _currentRecord)) return;
            Debug.Log("RECORD INDEX " + _currentRecordIndex);

            if (points != 0)
            {
                WindowsManager.Instance.CreateWindow<ReputationWindow>();
                WindowsManager.Instance.CreateWindow<ReputationWindow>()?.SetText(points);
            }

            GoTexNextScreenSetup();
        }

        private void SelectChainOption(int i)
        {
            Debug.LogWarning("Selecting Option: " + i);
            if (!_storyProgress.SelectOption(i, _currentRecord)) return;
            _selectedChainIndex = $"{i + 1}";
            optionWindow.DeleteAllOptionButtons();
            string cqline = _currentRecord.options[i].qline[0];
            _currentRecordIndex = _act.records.FindIndex(q =>
                q.qline != null && q.qline.Length > 0 && q.qline[0].StartsWith(cqline));
            GoTexNextScreenSetup();
        }


        private void Update()
        {
            //debug window creation
            if (Input.touchCount == 4 && _debugWindow == null)
            {
                _debugWindow = WindowsManager.Instance.CreateWindow<DebugWindow>();
                _debugWindow.SetStoryScreenReference(this);
            }

            // #if UNITY_EDITOR
            // if ((Input.GetKeyUp(KeyCode.Space) || Input.GetMouseButtonDown(1)) && _currentRecord.type == "text" &&
            //     !_debugWindow.isActivate)
            // {
            //     GoTexNextScreenSetup();
            // }
            // #endif

            // if (Input.GetTouch(0).phase == TouchPhase.Began && Input.touchCount==1 &&
            //  _currentRecord.type == "text" && !_debugWindow.isActivate)
            // {
            //     GoTexNextScreenSetup();
            // }

            if (PlayerPrefs.GetString("CloseWardrobe") == "true")
            {
                characterContainer.ShowHero(_currentRecord);
                PlayerPrefs.SetString("CloseWardrobe", "false");
            }

            SoundCheck();
        }

        public void OnSubMenuScreen()
        {
            subMenuButtons.SetActive(!subMenuButtons.activeSelf);
        }

        public void OnDressingroomButton()
        {
            _storyProgress.recordId = _currentRecordIndex;
            _storyProgress.actId = _act.id;
            GameManager.Instance.userData = _user;
            FirebaseManager.Instance.UpdateUserData();
            //ES3.Save("user", _user);

            if (_storyProgress.IsHeroCreated)
            {
                WindowsManager.Instance.CreateWindow<DressingRoomWindow>()
                    .ChooseWindowType(DressingRoomWindow.WindowType.Wardrobe);
            }
        }

        public void SoundCheck()
        {
            if (PlayerPrefs.GetString("Volume") == "false")
            {
                soundOff.SetActive(true);
                if (_currentRecord.sound != null)
                    GameManager.Instance.soundManager.Mute(true);
            }
            else
            {
                soundOn.SetActive(true);
                if (_currentRecord.sound != null)
                    GameManager.Instance.soundManager.Mute(false);
            }
        }

        public void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                SaveProgress();
            }
        }

        public void OnApplicationFocus(bool focusStatus)
        {
            if (!focusStatus)
            {
                SaveProgress();
            }
        }

        IEnumerator ChangeSound()
        {
            GameManager.Instance.soundManager.DoNull(nextBackgroundTransitionTime);
            yield return new WaitForSeconds(1f);
            _currentSound = GameManager.Instance.soundManager.PlaySounds(_currentRecord.sound,
                nextBackgroundTransitionTime, nextBackgroundTransitionTime);
            yield break;
        }
    }
}