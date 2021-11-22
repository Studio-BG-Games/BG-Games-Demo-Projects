using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Scripts.Managers;

public class DialogWindow : MonoBehaviour
{
    public event Action OnDialogButtonClick;
    [SerializeField] private GameObject npcNameContainer;
    [SerializeField] private GameObject npcArrow;
    [SerializeField] private TextMeshProUGUI npcNameText;

    [SerializeField] private GameObject protagonistNameContainer;
    [SerializeField] private GameObject protagonistArrow;
    [SerializeField] private TextMeshProUGUI protagonistNameText;

    [SerializeField] private string defaultPlayerName = "PlayerName";
    [SerializeField] private Button dialogButton;
    [SerializeField] private Image dialogWindow;
    [SerializeField] private TextMeshProUGUI dialogText;

    [SerializeField] private GameObject optionsWindow;
    [SerializeField] private RectTransform dialogWindowRectTransform;
    [SerializeField] private string nameHidden = "???";
    private bool _isOptionWindowEnabled;
    private string _playerName;

    private void Start()
    {
        dialogButton.onClick.AddListener(OnDialogClick);
    }

    private void OnDialogClick()
    {
        OnDialogButtonClick?.Invoke();
    }
    

    public void UpdateText(string text, string characterNamePar, string playerName,string[] npcModifications)
    {
        text = text.Replace(defaultPlayerName, playerName);
        dialogText.SetText(text);
        
        switch (characterNamePar)
        {
            case null:
                npcNameContainer.SetActive(false);
                npcArrow.SetActive(false);
                protagonistNameContainer.SetActive(false);
                protagonistArrow.SetActive(false);
                break;
            case "protagonist":
                npcNameContainer.SetActive(false);
                npcArrow.SetActive(false);
                protagonistNameContainer.SetActive(true);
                protagonistArrow.SetActive(true);
                protagonistNameText.SetText(playerName);
                break;
            default:
                npcNameContainer.SetActive(true);
                npcArrow.SetActive(true);
                protagonistNameContainer.SetActive(false);
                protagonistArrow.SetActive(false);
                var npcName = characterNamePar;

                if (npcModifications != null)
                {
                    foreach (var modificator in npcModifications)
                    {
                        if (modificator == "hidden")
                        {
                            npcName =nameHidden;
                        }
                    }
                }
                var _selectedStory = GameManager.Instance.selectedStory;        
                var itemGeneral = _selectedStory.general.Find(value => value.system == npcName);
                switch (PlayerPrefs.GetString("language"))
                {
                    case "rus": 
                        npcName = itemGeneral.russian;
                        break;
                    case "eng":
                        npcName = itemGeneral.english;
                        break;
                }
                npcNameText.SetText(npcName);
                break;
        }

        //HACK
        LayoutRebuilder.ForceRebuildLayoutImmediate(dialogWindowRectTransform);
    }

    public void ShowOptionsWindow()
    {
        _isOptionWindowEnabled = true;
        optionsWindow.SetActive(true);
    }

    public void HideOptionsWindow()
    {
        if (!_isOptionWindowEnabled) return;
        _isOptionWindowEnabled = false;
        optionsWindow.SetActive(false);
    }

    public void SetDialogButtonState(bool state)
    {
        dialogButton.gameObject.SetActive(state);
    }
}