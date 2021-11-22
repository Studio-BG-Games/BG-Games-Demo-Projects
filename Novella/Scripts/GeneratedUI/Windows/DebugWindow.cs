using System;
using Scripts.Managers;
using Scripts.Serializables.User;
using Scripts.UISystem;
using GeneratedUI;
using TMPro;
using UnityEngine;
using Scripts;

public class DebugWindow : WindowController
{
    public bool isActivate;
    [SerializeField] private GameObject actAndRecordsStuff;
    [SerializeField] private GameObject cashStuff;
    [SerializeField] private GameObject other;
    [SerializeField] private GameObject showDebugMenuButton;

    [SerializeField] private TMP_InputField dialogInputField;
    [SerializeField] private TMP_InputField cashInputField;
    [SerializeField] private TMP_InputField actInputField;

    [SerializeField] private TextMeshProUGUI cashText;
    [SerializeField] private TextMeshProUGUI currentDialogIndex;
    [SerializeField] private TextMeshProUGUI currentActIndex;

    //private PlayerInformation PlayerInformation => GameManager.Instance.playerInformation;



    private StoryScreen _storyScreen;
    private bool _isUnityEditor;

    private void Start()
    {
#if UNITY_EDITOR
        _isUnityEditor = true;
        actAndRecordsStuff.SetActive(false);
        cashStuff.SetActive(false);
        other.SetActive(false);
        showDebugMenuButton.SetActive(true);
        UpdateText();
#endif
    }

    public void SetStoryScreenReference(StoryScreen screen)
    {
        _storyScreen = screen;
        UpdateText();
    }

    private void UpdateText()
    {
        var userCurrency = GameManager.Instance.userData.currencies;
        var cash = userCurrency.cash;
        var cocktails = userCurrency.cocktails;
        var elixirs = userCurrency.elixirs;
        //var cash = PlayerInformation.cash;
        var dialogIndex = _storyScreen.GetDialogIndex();
        var actIndex = _storyScreen.GetActIndex();

        cashText.SetText($"cocktails:{cocktails}\ncash:{cash}\nelixirs:{elixirs}");
        currentDialogIndex.SetText($"{dialogIndex}");
        currentActIndex.SetText($"{actIndex}");
    }

    public void AddCash()
    {
        if (!int.TryParse(cashInputField.text, out var cash)) return;
        //PlayerInformation.cash += cash;

        ref var user = ref GameManager.Instance.userData;
        user.currencies.cash += cash;
        user.currencies.cocktails += cash;
        user.currencies.elixirs += cash;
        user.progress = _storyScreen._user.progress;
        FirebaseManager.Instance.UpdateUserData();
        //ES3.Save("user", user);
        UpdateText();
    }

    public void UseMagic()
    {
        ref var user = ref GameManager.Instance.userData;
        if(Int32.TryParse(GameManager.Instance.selectedStory.id, out var storyId))
        {
            if(user.progress[storyId].reputation.ContainsKey("reputation"))
            {
                user.progress[storyId].reputation["reputation"] += 10;
            }
            else
            {
                user.progress[storyId].reputation.Add("reputation", 10);
            }
            if(user.currencies.elixirs >= 1)
            {
                user.currencies.elixirs -= 1;
                user.progress[storyId].isElixirOn=true;            
                UpdateText();
            }
        }
        
    }

    public void ResetCurrencies()
    {
        ref var user = ref GameManager.Instance.userData;
        user.currencies.cash = 0;
        user.currencies.cocktails = 0;
        user.currencies.elixirs = 0;        
        UpdateText();
        
    }


    public void GoToDialog()
    {
        if (!int.TryParse(dialogInputField.text, out var index)) return;
        _storyScreen.ChangeDialogIndex(index);
        UpdateText();
    }

    public void GoToActAndDialog()
    {
        GoToAct();
        GoToDialog();
    }
    

    public void GoToAct()
    {
        if (!int.TryParse(actInputField.text, out var index)) return;
        _storyScreen.ChangeActIndex(index);
        UpdateText();
    }

    public void ShowDebugWindow()
    {
        isActivate = true;
        actAndRecordsStuff.SetActive(true);
        cashStuff.SetActive(true);
        other.SetActive(true);
        showDebugMenuButton.SetActive(false);
        UpdateText();
    }

    public void CloseDebugWindow()
    {
        if (_isUnityEditor)
        {
            actAndRecordsStuff.SetActive(false);
            cashStuff.SetActive(false);
            other.SetActive(false);
            showDebugMenuButton.SetActive(true);
        }
        else
        {
            CloseWindow();
        }

        isActivate = false;
    }
}