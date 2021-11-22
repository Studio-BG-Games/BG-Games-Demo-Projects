using System;
using System.Collections;
using System.Collections.Generic;
using GeneratedUI;
using Scripts;
using Scripts.Serializables.Story;
using Scripts.Serializables.User;
using Scripts.Tools.Actions;
using Scripts.UISystem;
using GeneratedUI;
using JetBrains.Annotations;
using UnityEngine;

namespace Scripts.Managers
{
    public class GameManager : SceneSingleton<GameManager>
    {
        private ActionSequence _startupSequence;
        public event System.Action OnLoadUserDataComplete;
        public List<Story> stories;
        public Story selectedStory;
        public Act selectedAct;
        public User userData;
        public PlayerInformation playerInformation;
        public ScriptableBase scriptableBase;
        public EffectsController effectsController;
        public SoundManager soundManager;

        private void Start()
        {
            if(PlayerPrefs.GetString("language") == "")
               PlayerPrefs.SetString("language", "rus");

            _startupSequence = new ActionSequence("Start Up Sequence", this, new Tools.Actions.Action[]
            {
                // Internet Check
                new CheckInternetConnection { name = "Check Internet Connection"},
                // Init Firebase
                new InitializeFirebase { name = "Initialize Firebase"},
                // Version Check and Remote Config
                new FetchRemoteConfig { name = "Fetching Remote Config"},
                // TODO: Asset Bundle Load/Update
                // Init Auth & Check
                new WaitForAuth { name = "Checking Auth" },
                // Load User Data
                new WaitForUserData { name = "Loading User Data"},
                // Load User data and Stories
                new LoadStories { name = "Loading Stories"}
            });

            _startupSequence.OnSequenceComplete += LoadUserData;
            _startupSequence.StartSequence();
            
            
            
            SDKManager.Instance.SetRewardVideoCallback(() =>
            {
                //here we give cash
                ref var user = ref GameManager.Instance.userData;
                user.currencies.cash += 2;
                WindowsManager.Instance.SearchForScreen<MenuScreen>()?.UpdateCurrencyBar();
                FirebaseManager.Instance.UpdateUserCurrencies();
            });
        }
        private void LoadUserData()
        {
            if(ES3.KeyExists("user"))
            {
                //userData = (User) ES3.Load("user");
            }
            OnLoadUserDataComplete?.Invoke();
        }

        private void OnDestroy()
        {
            _startupSequence.OnSequenceComplete -= LoadUserData;
        }
    }
}
