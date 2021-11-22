using UnityEngine;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using System;


public class AdmobAds : MonoBehaviour
{
    string GameID = "XXXXXXXX";

    // Sample ads
    string bannerAdId = "ca-app-pub-3940256099942544/6300978111";
    string InterstitialAdID = "ca-app-pub-3940256099942544/1033173712";
    string rewarded_Ad_ID = "ca-app-pub-3940256099942544/5224354917";


    public BannerView bannerAd;
    public InterstitialAd interstitial;
    public RewardedAd rewardedAd;


    public static AdmobAds instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        // DontDestroyOnLoad(this);

        //rewardedAd = RewardedAd.Instance;
    }

    // Start is called before the first frame update
    void Start()
    {
       // MobileAds.Initialize(GameID);

    }


    public void loadRewardVideo()
    {
        //rewardedAd.LoadAd(new AdRequest.Builder().Build(), rewarded_Ad_ID);


        rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        rewardedAd.OnAdClosed += HandleRewardedAdClosed;
        rewardedAd.OnAdOpening += HandleRewardedAdOpening;
        rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
        rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;

    }

    AdRequest AdRequestBuild()
    {
        return new AdRequest.Builder().Build();
    }

    /// rewarded video events //////////////////////////////////////////////

    public event EventHandler<EventArgs> OnAdLoaded;

    public event EventHandler<AdFailedToLoadEventArgs> OnAdFailedToLoad;

    public event EventHandler<EventArgs> OnAdOpening;

    public event EventHandler<EventArgs> OnAdStarted;

    public event EventHandler<EventArgs> OnAdClosed;

    public event EventHandler<Reward> OnAdRewarded;

    public event EventHandler<EventArgs> OnAdLeavingApplication;

    public event EventHandler<EventArgs> OnAdCompleted;

    /// Rewared events //////////////////////////



    public void HandleRewardedAdLoaded(object sender, EventArgs args)
    {
        Debug.Log("Video Loaded");
    }

    public void HandleRewardedAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        Debug.Log("Video not loaded");
    }

    public void HandleRewardedAdOpening(object sender, EventArgs args)
    {
        Debug.Log("Video Loading");
    }

    public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        Debug.Log("Video Loading failed");
    }

    public void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        Debug.Log("Video Loading failed");
    }

    public void HandleUserEarnedReward(object sender, Reward args)
    {
        /// reward the player here --------------------
        Debug.Log("Player Rewarded");
        //GameManager.GMinstance.rewaredPlayer();

    }

    public void HandleOnRewardAdleavingApp(object sender, EventArgs args)
    {
        Debug.Log("when user clicks the video and open a new window");
    }



    public void showVideoAd()
    {
        if (rewardedAd.IsLoaded())
        {
            rewardedAd.Show();
        }
        else
        {
            Debug.Log("Rewarded Video ad not loaded");
        }
    }
}
