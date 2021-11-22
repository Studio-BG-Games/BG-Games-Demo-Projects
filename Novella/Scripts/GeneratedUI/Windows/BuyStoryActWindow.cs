using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.UISystem;
using Scripts.Managers;
using Scripts.Serializables.User;
using GeneratedUI;
using TMPro;
using Scripts;

public class BuyStoryActWindow : WindowController
{
    [SerializeField] private TextMeshProUGUI localizedDescription1;
    [SerializeField] private TextMeshProUGUI localizedDescription2;
    [SerializeField] private TextMeshProUGUI localizedDescription3;
    [SerializeField] private TextMeshProUGUI buttonText;
    [SerializeField] private TextMeshProUGUI button_2Text;
    [SerializeField] private TextMeshProUGUI closeWindowButton;
    private StoryWindow _storyWindow;
    public event System.Action OnBuyComplete;

    private void Start()
    {        
        OnBuyComplete += WindowCompleteDealEvent;
        //No translation in "general" table for this prefab
        switch (PlayerPrefs.GetString("language"))
        {
            case "rus": 
                localizedDescription1.text = "Вы можете открыть серию. Для этого вам понадобится";
                localizedDescription2.text = "или";
                localizedDescription3.text = "При открытии за эликсир платные ответы серии станут бесплатны";
                buttonText.text = "Открыть серию за";
                button_2Text.text = "Открыть серию за";
                closeWindowButton.text = "Закрыть";
                break;
            case "eng":
                localizedDescription1.text = "You can open the series. For this you need";
                localizedDescription2.text = "or";
                localizedDescription3.text = "When opened for the elixir, the paid series answers are free";
                buttonText.text = "Open the series for";
                button_2Text.text = "Open the series for";
                closeWindowButton.text = "Close";
                break;
        }
    }

    public void BuyActForCocktail()
    {        
        ref var user = ref GameManager.Instance.userData;
        var selectedStory = GameManager.Instance.selectedStory;
        var story_progress = user.progress.Find(value => value.storyId == selectedStory.id);
        if(user.currencies.cocktails < 1)
        {
            WindowsManager.Instance.CreateWindow<NoMoneyPopupWindow>();
            return; 
        }
        else
        {
            if(story_progress==null)
            {
                story_progress = new StoryProgress(GameManager.Instance.selectedStory.id);
                user.progress.Add(story_progress);  
            } 
            Payment(ref user, story_progress, 1);          
        }
        CloseWindow();
    }
    public void BuyActForElixir()
    {  
        ref var user = ref GameManager.Instance.userData;
        var selectedStory = GameManager.Instance.selectedStory;
        var t = GameManager.Instance.selectedStory;
        var story_progress = user.progress.Find(value => value.storyId == selectedStory.id);
        if(user.currencies.elixirs < 1)
        {            
            WindowsManager.Instance.CreateWindow<NoMoneyPopupWindow>();            
            return; 
        }
        else
        {
            if(story_progress==null)
            {
                story_progress = new StoryProgress(GameManager.Instance.selectedStory.id);
                user.progress.Add(story_progress);  
            } 
            Payment(ref user, story_progress, 2);                      
        }
        CloseWindow();
    }

    public void Payment(ref User user, StoryProgress progress, int paymentСhoice)
    {                
        switch (paymentСhoice)
        {
            case 1:
                user.currencies.cocktails -= 1;
                progress.lastPurchasedAct = progress.actId;     
                break;
            case 2:
                progress.isElixirOn = true;
                user.currencies.elixirs -= 1;
                progress.lastPurchasedAct = progress.actId;     
                break;
            default:
                break;
        }
        
        // if(GameObject.Find("StoryWindow(Clone)") != null)
        // {
        //     GameObject.Find("StoryWindow(Clone)").GetComponent<StoryWindow>().GetBuyStoryActWindowInstance(this);
        //     OnBuyComplete?.Invoke();
        // }
        if(WindowsManager.Instance.SearchForWindow<StoryWindow>() != null)
        {
            WindowsManager.Instance.SearchForWindow<StoryWindow>().GetWindowInstance(this);
            OnBuyComplete?.Invoke();
        }
    }

    private void OnDestroy() 
    {
        OnBuyComplete -= WindowCompleteDealEvent;    
    } 
    
}
