using System;
using System.Collections;
using System.Collections.Generic;
using Scripts.Managers;
using Scripts.Serializables.User;
using Scripts.Serializables.Story;
using Scripts.UISystem;
using GeneratedUI;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Scripts;

public class GameOverWindow : WindowController
{
    [SerializeField] private TextMeshProUGUI elixir, elixirsLocalization, rubies, reputation, procced;
    [SerializeField] private TextMeshProUGUI additionalRubiesText, addedReputation;
    [SerializeField] private Image gameOver;
    [SerializeField] private Sprite rusGameOver, engGameOver;

    [SerializeField] private Button _rewardButton;

    private General diamondsGeneral, reputationGeneral, elixirsGeneral, proccedGeneral;
    private int reputationForAct;

    public string diamondsKey = "diamonds", reputationKey="reputation", elixirsKey="elixirs",proceedKey="procced";
    private void Start() 
    {
        var selectedStory = GameManager.Instance.selectedStory;        
        diamondsGeneral = selectedStory.general.Find(value => value.system == diamondsKey);
        reputationGeneral = selectedStory.general.Find(value => value.system == reputationKey);
        elixirsGeneral = selectedStory.general.Find(value => value.system == elixirsKey);
        proccedGeneral = selectedStory.general.Find(value => value.system == proceedKey);
        if (PlayerPrefs.GetString("language") == "rus")
        {
            elixirsLocalization.text = elixirsGeneral.russian;
            procced.text = proccedGeneral.russian;
            gameOver.sprite = rusGameOver;
        }
        else if (PlayerPrefs.GetString("language") == "eng")
        { 
            elixirsLocalization.text = elixirsGeneral.english;
            procced.text = proccedGeneral.english;
            gameOver.sprite = engGameOver;
        }

        _rewardButton.interactable = true;
    }

    public void GameOver()
    {
        GameManager.Instance.effectsController.PlayFadeEffect(1,null,LoadMenuScene);
    }
    
    private void LoadMenuScene()
    {
        WindowsManager.Instance.CreateScreen<MenuScreen>();
        WindowsManager.Instance.SearchForScreen<MenuScreen>()?.PopulateStories();
        GameManager.Instance.soundManager.StopCurrentSound(2.5f);
        WindowsManager.Instance.CloseAllWindows();
    }

    public void OutputOfResults(StoryProgress currentProgress) 
    {
        // (30) currentProgress.reputation["reputation"] 
        // currentProgress.reputation["reputation"]  - currentProgress.globalReputationBeforeAct  (15)
        ref var user = ref GameManager.Instance.userData;
        elixir.text = user.currencies.elixirs.ToString();
        rubies.text = user.currencies.cash.ToString();

        if (currentProgress.lastPurchasedAct == 0 && PlayerPrefs.GetString("GetCash") != "true")
        {
            user.currencies.cash += 10;
            FirebaseManager.Instance.UpdateUserCurrencies();
            //ES3.Save("user", user);
            StartCoroutine(ShowResultRubies());
            PlayerPrefs.SetString("GetCash","true");
        }

        StartCoroutine(ShowResultReputation(currentProgress));
    }

    public void ShowRewardedVideo()
    {
        if (SDKManager.Instance.RewardVideoAvailable)
        {
            _rewardButton.interactable = false;
        }
        SDKManager.Instance.PlayRewardedVideo();
    }

    IEnumerator ShowResultRubies()
    {
        for (int i = 1; i <= 10; i++)
        {
            if(PlayerPrefs.GetString("language")=="rus")
                additionalRubiesText.text = " (+" + i + ") " + diamondsGeneral.russian;
            else if(PlayerPrefs.GetString("language")=="eng")
                additionalRubiesText.text = " (+" + i + ") " + diamondsGeneral.english;
            rubies.text = (Int32.Parse(rubies.text) + 1).ToString();
            yield return new WaitForSeconds(0.15f);
        }
        yield break;
    }

    IEnumerator ShowResultReputation(StoryProgress currentProgress)
    {
        if(!currentProgress.reputation.ContainsKey("reputation"))
        {
            currentProgress.reputation.Add("reputation", 10);
        }
        reputationForAct = currentProgress.reputation["reputation"] - currentProgress.globalReputationBeforeAct;
        int reputations = currentProgress.globalReputationBeforeAct;
        if(currentProgress.reputation.ContainsKey("reputation"))
            currentProgress.globalReputationBeforeAct = currentProgress.reputation["reputation"]; 
        

        Debug.Log("currentProgress.reputation" + currentProgress.reputation["reputation"]);
        Debug.Log("currentProgress.globalReputationBeforeAct" + currentProgress.globalReputationBeforeAct);

        for (int i = 1; i <= Math.Abs(reputationForAct); i++)
        {
            if (reputationForAct > 0)
            {
                reputations += 1;
                reputation.text = reputations.ToString();
                if (PlayerPrefs.GetString("language") == "rus")
                    addedReputation.text = "(+" + i + ") " + "Репутации";
                else if (PlayerPrefs.GetString("language") == "eng")
                    addedReputation.text = "(+" + i + ") " + "Reputation";       
            }

            if (reputationForAct < 0)
            {
                reputations -= 1;
                if (reputations < 0) yield break;
                reputation.text = reputations.ToString();
                if(PlayerPrefs.GetString("language")=="rus")
                    addedReputation.text = "(-" + i + ") " + "Репутации";
                else if(PlayerPrefs.GetString("language")=="eng")
                    addedReputation.text = "(-" + i + ") " +"Reputation";
            }

            yield return new WaitForSeconds(0.25f);
        }

        if (Math.Abs(reputationForAct) == 0)
        {
            reputation.text = reputations.ToString();
            if (PlayerPrefs.GetString("language") == "rus")
                addedReputation.text = "(" + 0 + ") " + "Репутации";
            else if (PlayerPrefs.GetString("language") == "eng")
                addedReputation.text = "(" + 0 + ") " + "Reputation";
        } 

        yield break;
    }
}
