using System.Collections;
using System.Collections.Generic;
using Scripts.UISystem;
using Scripts.Managers;
using Scripts.Serializables.Story;
using Scripts.Serializables.User;
using UnityEngine;
using Scripts;

namespace GeneratedUI
{
    public class SubMenuPanel : WindowController
    {
        [SerializeField] private GameObject soundOff, soundOn;

        private Story _story;
        private Act _act;
        private User _user;
        private Story _selectedStory;
        private Record _currentRecord;
        private StoryProgress _storyProgress;
        private int _currentRecordIndex;

        private void Start()
        {
            SetCurrentRecord();
            SoundCheck();
        }

        private void SetCurrentRecord()
        {
            _story = GameManager.Instance.selectedStory;
            _user = GameManager.Instance.userData;
            _selectedStory = GameManager.Instance.selectedStory;
            _storyProgress = _user.progress.Find(value => value.storyId == _selectedStory.id);
            _act = _story.acts.Find(a => a.id == _storyProgress.actId);
            _currentRecordIndex = _storyProgress.recordId;
            _currentRecord = _act.records[_currentRecordIndex];
        }

        public void OnLeftButon()
        {
            WindowsManager.Instance.CreateWindow<LoadingWindow>();
            WindowsManager.Instance.SearchForWindow<LoadingWindow>()?.HideUnusedObjects();
            WindowsManager.Instance.CreateScreen<MenuScreen>();
            WindowsManager.Instance.SearchForScreen<MenuScreen>()?.PopulateStories();
            GameManager.Instance.soundManager.StopCurrentSound(2.5f);
            //_storyProgress.recordId = _currentRecordIndex; // лишнее т.к. при закрытии истории есть вызов сохранения прогресса
            //_storyProgress.actId = _act.id;
            //GameManager.Instance.userData = _user;
            //ES3.Save("user", GameManager.Instance.userData);
            CloseWindow();
        }

        public void OnSoundButton()
        {
            if (PlayerPrefs.GetString("Volume") != "false")
            {
                PlayerPrefs.SetString("Volume", "false");
                soundOff.SetActive(true);
                soundOn.SetActive(false);
                if (_currentRecord.sound != null)
                    GameManager.Instance.soundManager.Mute(true);
            }
            else
            {
                PlayerPrefs.SetString("Volume", "true");
                soundOn.SetActive(true);
                soundOff.SetActive(false);
                if (_currentRecord.sound != null)
                    GameManager.Instance.soundManager.Mute(false);
            }
        }

        public void SoundCheck()
        {
            if (PlayerPrefs.GetString("Volume") == "false")
            {
                soundOff.SetActive(true);
            }
            else
            {
                soundOn.SetActive(true);
            }
        }
    }
}
