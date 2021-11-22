using System.Collections;
using System.Collections.Generic;
using Scripts.UISystem;
using Scripts.Serializables.User;
using Scripts.Serializables.Story;
using Scripts.Managers;
using UnityEngine;
using System;
using TMPro;
using GeneratedUI;
using Scripts;

public class ReplayStoryWindow : WindowController
{
    [SerializeField] private TextMeshProUGUI descriptionTMP1, descriptionTMP2, buttonTMP1, buttonElixitTMP1, buttonTMP2, buttonElixitTMP2, closeButtonTMP;
    public event System.Action OnReplayComplete;
    private string desc1Key = "restart_desc1", desc2Key = "restart_desc2", button1Key = "restart_act", button2Key = "restart_story", closeButtonKey = "close";
    private string descElixir1Key = "restart_act", descElixir2Key = "restart_story";
    private bool isBeingReplayedForElxir;
    private void Start()
    {        
        OnReplayComplete += WindowCompleteDealEvent;
        var selectedStory = GameManager.Instance.selectedStory;        
        var description1 = selectedStory.general.Find(value => value.system == desc1Key);
        var description2 = selectedStory.general.Find(value => value.system == desc2Key);
        var descriptionActCocktail1 = selectedStory.general.Find(value => value.system == button1Key);        
        var descriptionStoryCocktail2 = selectedStory.general.Find(value => value.system == button2Key);
        var descriptionActElixir1 = selectedStory.general.Find(value => value.system == descElixir1Key);
        var descriptionStoryElixir2 = selectedStory.general.Find(value => value.system == descElixir2Key);
        var closeButton = selectedStory.general.Find(value => value.system == closeButtonKey);
        switch(PlayerPrefs.GetString("language"))
        {
			case "rus":
				descriptionTMP1.text = description1.russian;                
                descriptionTMP2.text = description2.russian;                
                buttonTMP1.text = descriptionActCocktail1.russian; 
                buttonElixitTMP1.text = descriptionActElixir1.russian;               
                buttonTMP2.text = descriptionStoryCocktail2.russian; 
                buttonElixitTMP2.text = descriptionStoryElixir2.russian;               
				closeButtonTMP.text = closeButton.russian;
                break;
			case "eng":
				descriptionTMP1.text = description1.english;                
                descriptionTMP2.text = description2.english;                
                buttonTMP1.text = descriptionActCocktail1.english;
                buttonElixitTMP1.text = descriptionActElixir1.english;
                buttonTMP2.text = descriptionStoryCocktail2.english;
                buttonElixitTMP2.text = descriptionStoryElixir2.english;
                closeButtonTMP.text = closeButton.english;
				break;
		}

    }
    public void ReplayAct()
    {
        ref var user = ref GameManager.Instance.userData;
        if(!isBeingReplayedForElxir)
        {
            if(user.currencies.cocktails < 1)
            {
                WindowsManager.Instance.CreateWindow<NoMoneyPopupWindow>();
                return;
            }  
        }
        else
        {
            if(user.currencies.elixirs < 1)
            {
                WindowsManager.Instance.CreateWindow<NoMoneyPopupWindow>();
                return;
            } 
        }
           
        ref var selectedStory = ref GameManager.Instance.selectedStory; 
        if(Int32.TryParse(selectedStory.id, out var storyId))
        {          
            var progress = user.progress[storyId];
            if(progress.actId==0)
            {                
                ReplayStory();
                return;
            } 

            if(progress.qlinesActLast is null)
            {
                progress.qlinesActLast = new List<string>();
            }
            else
            {                              
                progress.qlinesActCurrent = new List<string>();
                progress.reputation = new Dictionary<string, int>(progress.lastActReputation);                
            }
            progress.heroDressKey = progress.heroDressKeyBeforeAct;
            progress.heroHairKey = progress.heroHairKeyBeforeAct;
            progress.qlinesActCurrent = progress.actQlines;
            while (progress.qlines.Count <= progress.actId) progress.qlines.Add(new List<string>());//в скипе актов через дебаг окно аналогичная логика, а для стандартного прохождения это не нужно
            progress.qlines[progress.actId] = new List<string>(progress.qlinesActLast);

            progress.recordId = 0;
            if(!isBeingReplayedForElxir)
            {
                user.currencies.cocktails -= 1;
                progress.isElixirOn = false;
            }
            else
            {
                user.currencies.elixirs -= 1; 
                progress.isElixirOn = true;
            }                
            
            FirebaseManager.Instance.UpdateUserData();
            //ES3.Save("user", GameManager.Instance.userData);

            if(WindowsManager.Instance.SearchForWindow<StoryWindow>() != null)
            {
                WindowsManager.Instance.SearchForWindow<StoryWindow>().GetWindowInstance(this);
                OnReplayComplete?.Invoke();
            }        
        }
        else
        {
            Debug.LogWarning($"Can't parse selectedStory.id: {selectedStory.id} to Int32");            
        }        
        CloseWindow();                   
    }
    public void ReplayActForElixir()
    {
        isBeingReplayedForElxir = true;
        ReplayAct();
    }
    public void ReplayStory()
    {
        var dupUser =  GameManager.Instance.userData;
        if(!isBeingReplayedForElxir)
        {
            if(dupUser.currencies.cocktails < 1)
            {
                WindowsManager.Instance.CreateWindow<NoMoneyPopupWindow>();
                return;    
            } 
        }
        else
        {
            if(dupUser.currencies.elixirs < 1)
            {
                WindowsManager.Instance.CreateWindow<NoMoneyPopupWindow>();
                return;    
            } 
        }
        var selectedStory = GameManager.Instance.selectedStory;    
        var dupProgress = dupUser.progress.Find(value => value.storyId == selectedStory.id);        
        var dupPaidQlinesInfo = dupProgress.paidQlines;
        var dupPaidItems = dupProgress.paidItems;
        var dupPaidActs = dupProgress.lastPurchasedAct;
                
        if(Int32.TryParse(selectedStory.id, out var storyId))
        {
            ref var user = ref GameManager.Instance.userData;
            user.progress[storyId] = new StoryProgress(selectedStory.id);
            var progress = user.progress[storyId];
            if(!isBeingReplayedForElxir)
            {
                user.currencies.cocktails -= 1;
                progress.isElixirOn = false;   
            }  
            else
            {
                user.currencies.elixirs -= 1;
                progress.isElixirOn = true;   
            }       
            progress.paidQlines= dupPaidQlinesInfo;
            progress.paidItems = dupPaidItems;
            progress.lastPurchasedAct = 0;
            progress.reputation = new Dictionary<string, int>();
            progress.lastActReputation = new Dictionary<string, int>();
            FirebaseManager.Instance.UpdateUserData();
            //ES3.Save("user", GameManager.Instance.userData);

            if(WindowsManager.Instance.SearchForWindow<StoryWindow>() != null)
            {
                WindowsManager.Instance.SearchForWindow<StoryWindow>().GetWindowInstance(this);
                OnReplayComplete?.Invoke();
            }  
        }
        else
        {
            Debug.LogWarning($"<color=red>Can't parse selectedStory.id: {selectedStory.id} to Int32</color>");            
        }
        CloseWindow();         
    }
    public void ReplayStoryForElixir()
    {
        isBeingReplayedForElxir = true;
        ReplayStory();
    }
    private void OnDestroy() 
    {
        OnReplayComplete -= WindowCompleteDealEvent;
    }
}
