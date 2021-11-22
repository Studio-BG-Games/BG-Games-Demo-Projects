using Scripts.Managers;
using Scripts.Serializables;
using Scripts.Serializables.Story;
using Scripts.Serializables.User;
using Scripts.UISystem;
using GeneratedUI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Menu
{
    public class StoryCard : MonoBehaviour
    {
#pragma warning disable 0649
        [SerializeField] private Image image;
        [SerializeField] private TMP_Text title;
        [SerializeField] private Button button;
        private InputHandler _inputHandlerRef;

        private Story _story;
        private bool _isBlocking;
#pragma warning restore 0649

        public void Initialize(Story story)
        {
            if (title != null) title.text = story.title;
            _story = story;
        }

        public void OpenStory()
        {
            if (_isBlocking) return;
            // Open Story Window!
            _inputHandlerRef.enabled = false;
            GameManager.Instance.selectedStory = _story;
            var storyWindow = WindowsManager.Instance.CreateWindow<StoryWindow>();
            storyWindow.onWindowClose += () => _inputHandlerRef.enabled = true;
            UserDataCheck();
        }

        private void UserDataCheck()
        {
            LoadStoryProgress();
        }

        private void LoadStoryProgress()
        {
            var user = GameManager.Instance.userData;
            var selectedStory = GameManager.Instance.selectedStory;
            var storyProgress = user.progress.Find(value => value.storyId == selectedStory.id);

            Debug.Log(storyProgress == null ? "progress is null" : "progress found");
        }

        public void SetInputHandlerRef(InputHandler inputHandler)
        {
            _inputHandlerRef = inputHandler;
        }

        public void SetBlockState(bool state)
        {
            _isBlocking = state;
        }
    }
}