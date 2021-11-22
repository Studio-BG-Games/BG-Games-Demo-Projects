using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using MEC;
using System.Threading;


public class InterAdd : MonoBehaviour
{
    [SerializeField] private GameController gameController;

    public InterstitialAd interstitialAd;
    public RewardedAd rewardedAd;

    //private string interstitialID = "ca-app-pub-7635126548465920/7798919881"; //android
    //private string rewardedAdId = "ca-app-pub-7635126548465920/4692027351";

    //private string interstitialID = "ca-app-pub-7635126548465920/2736975521"; //ios
    //private string rewardedAdId = "ca-app-pub-7635126548465920/5363138867";

    private string interstitialID = "ca-app-pub-3940256099942544/1033173712";  //fake
    private string rewardedAdId = "ca-app-pub-3940256099942544/5224354917";
    AdSize adsize = new AdSize(300, 100);
    private bool rewardClosed = true;
    public bool rewardLoaded;
    public bool rewardRequested;
    Thread adThread;

    public void LoadOnDeathAd()
    {

        interstitialAd = new InterstitialAd(interstitialID);
        AdRequest adRequest = new AdRequest.Builder().Build();
        interstitialAd.LoadAd(adRequest);
    }

    private void Start()
    {
        gameController = GameObject.Find("EventSystem").GetComponent<GameController>();

        //RequestRewardedVideo();
        
    }

    public void ShowAd()
    {
        if(interstitialAd.IsLoaded())
            interstitialAd.Show();
    }

    public void ShowRewardedAd()
    {
        rewardedAd.Show();
    }




    public void RequestRewardedVideo()
    {
        rewardedAd = new RewardedAd(rewardedAdId);
        AdRequest request = AdRequestBuild();
        rewardedAd.LoadAd(request);
        rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
        rewardedAd.OnAdClosed += HandleRewardedAdClosed;

    }

    public event EventHandler<EventArgs> OnAdLoaded;

    public event EventHandler<AdFailedToLoadEventArgs> OnAdFailedToLoad;

    public event EventHandler<EventArgs> OnAdOpening;

    public event EventHandler<EventArgs> OnAdStarted;

    public event EventHandler<EventArgs> OnAdClosed;

    public event EventHandler<Reward> OnAdRewarded;

    
    public event EventHandler<EventArgs> OnAdLeavingApplication;

    public event EventHandler<EventArgs> OnAdCompleted;

    public void HandleUserEarnedReward(object sender, Reward args)
    {
        /// reward the player here --------------------
        //Debug.Log("Player Rewarded");
        gameController.ContinueGame();
        rewardClosed = false;
        //GameManager.GMinstance.rewaredPlayer();

    }

    public void HandleRewardedAdLoaded(object sender, EventArgs args)
    {
        if (rewardRequested)
            ShowRewardedAd();
        rewardLoaded = true;
        //rewardedAd.Show();
    }


    public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        Timing.RunCoroutine(gameController.RestartGame());

    }

    public void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        if(rewardClosed)
            Timing.RunCoroutine(gameController.RestartGame());
        rewardedAd.Destroy();

    }

    public void HandleRewardedAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        Debug.Log("Video not loaded");
        Debug.Log("rewardRequested" + rewardRequested);
        if (rewardRequested)
            Timing.RunCoroutine(gameController.RestartGame());
        gameController.failetoLoadRewardedAd = true;
    }

    

    AdRequest AdRequestBuild()
    {
        return new AdRequest.Builder().Build();
    }

    private void OnDestroy()
    {
        try
        {
            interstitialAd.Destroy();
            rewardedAd.Destroy();
        }
        catch (Exception)
        { }
    }


}
