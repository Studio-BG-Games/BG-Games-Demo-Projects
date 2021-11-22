using System;
using UnityEngine;
using GoogleMobileAds.Api;
using UnityEngine.Events;


public class AdMobHandler : MonoBehaviour
{
    public static AdMobHandler Instance;
    //public bool _isRewardEarned;
    private RewardedAd _rewardedAd;
    private InterstitialAd _interstitialAd;
    private string _rewardedAdUnitId, _interstitialAdUnitId;
    public event Action OnUserEarnedReward, OnAdClosed;
    //public UnityEvent OnUserEarnedRewardEvent;
    
    //private string idApp;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
        CreateAndLoadRewardedAd();
        CreateAndLoadInterstitialAd();
    }
    

    private void CreateAndLoadInterstitialAd()
    {
        #if UNITY_EDITOR
        _interstitialAdUnitId = "unused";
        #elif UNITY_ANDROID
        //_interstitialAdUnitId = "secret"; // Real id
        _interstitialAdUnitId = "ca-app-pub-3940256099942544/1033173712";// TEST Id
        #endif   
        
        _interstitialAd = new InterstitialAd(_interstitialAdUnitId);
        AdRequest request = new AdRequest.Builder().Build();
        
        _interstitialAd.OnAdLoaded += HandleOnAdLoaded;
        // Called when an ad request failed to load.
        _interstitialAd.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        // Called when an ad is shown.
        _interstitialAd.OnAdOpening += HandleOnAdOpened;
        // Called when the ad is closed.
        _interstitialAd.OnAdClosed += HandleOnAdClosed;
        
        _interstitialAd.LoadAd(request);
        
    }
    
    private void CreateAndLoadRewardedAd()
    {
        #if UNITY_EDITOR
        _rewardedAdUnitId = "unused";
        #elif UNITY_ANDROID
        //_rewardedAdUnitId = "secret"; // Real Id
        _rewardedAdUnitId = "ca-app-pub-3940256099942544/5224354917";// TEST Id
        #endif
        
        _rewardedAd = new RewardedAd(_rewardedAdUnitId);
        
        // Called when an ad request has successfully loaded.
        _rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        // Called when an ad request failed to load.
        _rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
        // Called when an ad is shown.
        _rewardedAd.OnAdOpening += HandleRewardedAdOpening;
        // Called when an ad request failed to show.
        _rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
        // Called when the user should be rewarded for interacting with the ad.
        _rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        // Called when the ad is closed.
        _rewardedAd.OnAdClosed += HandleRewardedAdClosed;
        
        //_rewardedAd.OnUserEarnedReward += (sender, args) => OnUserEarnedRewardEvent.Invoke();
        
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded ad with the request.
        _rewardedAd.LoadAd(request);
        print("Reward Ad created and loaded");
    }

    #region Reward Handle Methods
    private void HandleRewardedAdLoaded(object sender, EventArgs args)
    {
        print("HandleRewardedAdLoaded event received");
    }

    private void HandleRewardedAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        print(
            "HandleRewardedAdFailedToLoad event received with message: "
            + args.LoadAdError.GetMessage());
    }

    private void HandleRewardedAdOpening(object sender, EventArgs args)
    {
        print("HandleRewardedAdOpening event received");
    }

    private void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        print(
            "HandleRewardedAdFailedToShow event received with message: "
            + args.AdError.GetMessage());
    }

    private void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        print("HandleRewardedAdClosed event received");
        _rewardedAd.OnAdLoaded -= HandleRewardedAdLoaded;
        _rewardedAd.OnAdFailedToLoad -= HandleRewardedAdFailedToLoad;
        _rewardedAd.OnAdOpening -= HandleRewardedAdOpening;
        _rewardedAd.OnAdFailedToShow -= HandleRewardedAdFailedToShow;
        _rewardedAd.OnUserEarnedReward -= HandleUserEarnedReward;
        _rewardedAd.OnAdClosed -= HandleRewardedAdClosed;
        //_isRewardEarned = false;
        OnAdClosed?.Invoke();
        
        //Preload next Ad
        CreateAndLoadRewardedAd();
    }
    
    private void HandleUserEarnedReward(object sender, Reward args)
    {
        // Базово награда amount == 1, можно играть с этим, а не bool переключателем, ну или монет добавлять
        //_isRewardEarned = true;
        string type = args.Type;
        double amount = args.Amount;
        print(
            "HandleRewardedAdRewarded event received for "
            + amount.ToString() + " " + type);
        OnUserEarnedReward?.Invoke();
        
        //CreateAndLoadRewardedAd();
    }
    #endregion
    
    #region Interstitial Handle Methods
    public void HandleOnAdLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLoaded event received");
    }

    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        MonoBehaviour.print("HandleFailedToReceiveAd event received with message: "
                            + args.LoadAdError.GetMessage());
    }

    public void HandleOnAdOpened(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdOpened event received");
    }

    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdClosed event received");
        
        _interstitialAd.OnAdLoaded -= HandleOnAdLoaded;
        _interstitialAd.OnAdFailedToLoad -= HandleOnAdFailedToLoad;
        _interstitialAd.OnAdOpening -= HandleOnAdOpened;
        _interstitialAd.OnAdClosed -= HandleOnAdClosed;
        
        CreateAndLoadInterstitialAd();
    }
    
    #endregion
    
    public void ShowRewardedAd()
    {
        if (_rewardedAd.IsLoaded()) 
        {
            print("Reward Ad is loaded. Method Show() starts");
            _rewardedAd.Show();
        }
        else
        {
            print("Reward Ad is not loaded");
        }
    }

    public void ShowInterstitialAd()
    {
        if (_interstitialAd.IsLoaded()) 
        {
            print("Interstitial Ad is loaded. Method Show() starts");
            _interstitialAd.Show();
        }
        else
        {
            print("Interstitial Ad is not loaded");
        }  
    }
    
    private void OnDestroy ()
    {
        _rewardedAd.OnAdLoaded -= HandleRewardedAdLoaded;
        _rewardedAd.OnAdFailedToLoad -= HandleRewardedAdFailedToLoad;
        _rewardedAd.OnAdOpening -= HandleRewardedAdOpening;
        _rewardedAd.OnAdFailedToShow -= HandleRewardedAdFailedToShow;
        _rewardedAd.OnUserEarnedReward -= HandleUserEarnedReward;
        _rewardedAd.OnAdClosed -= HandleRewardedAdClosed;
        
        _interstitialAd.OnAdLoaded -= HandleOnAdLoaded;
        _interstitialAd.OnAdFailedToLoad -= HandleOnAdFailedToLoad;
        _interstitialAd.OnAdOpening -= HandleOnAdOpened;
        _interstitialAd.OnAdClosed -= HandleOnAdClosed;
    }
}
