using System;
using Scripts;
using UnityEngine;

public class SDKManager : SceneSingleton<SDKManager>
{
    [SerializeField] private string IronSourceAppKeyIOS = "********";
    [SerializeField] private string IronSourceAppKeyAndroid = "********";
    private string IronSourceAppKey = "********";


    private bool rewardVideoAvailable = false;
    private bool interstitialAvailable = false;

    public bool InterstitialAvailable => interstitialAvailable;

    public bool RewardVideoAvailable => rewardVideoAvailable;

    private Action rewardCallback;
    private Action _interstitialDone;

    #region setup

    public override void Awake()
    {
        base.Awake();
#if UNITY_IOS
        {
            IronSourceAppKey = IronSourceAppKeyIOS;
        }
#elif UNITY_ANDROID
        {
            IronSourceAppKey = IronSourceAppKeyAndroid;
        }
#endif
        Debug.Log($"IRONSOURCE API KEY: " + IronSourceAppKey);
        IronSource.Agent.init(IronSourceAppKey, IronSourceAdUnits.REWARDED_VIDEO);
        IronSource.Agent.init(IronSourceAppKey, IronSourceAdUnits.INTERSTITIAL);
        IronSource.Agent.validateIntegration();
    }


    void Start()
    {
        IronSourceEvents.onRewardedVideoAdOpenedEvent += RewardedVideoAdOpenedEvent;
        IronSourceEvents.onRewardedVideoAdClickedEvent += RewardedVideoAdClickedEvent;
        IronSourceEvents.onRewardedVideoAdClosedEvent += RewardedVideoAdClosedEvent;
        IronSourceEvents.onRewardedVideoAvailabilityChangedEvent += RewardedVideoAvailabilityChangedEvent;
        IronSourceEvents.onRewardedVideoAdStartedEvent += RewardedVideoAdStartedEvent;
        IronSourceEvents.onRewardedVideoAdEndedEvent += RewardedVideoAdEndedEvent;
        IronSourceEvents.onRewardedVideoAdRewardedEvent += RewardedVideoAdRewardedEvent;
        IronSourceEvents.onRewardedVideoAdShowFailedEvent += RewardedVideoAdShowFailedEvent;

        rewardVideoAvailable = IronSource.Agent.isRewardedVideoAvailable();
    }

    void OnEnable()
    {
        IronSourceEvents.onInterstitialAdReadyEvent += InterstitialAdReadyEvent;
        IronSourceEvents.onInterstitialAdLoadFailedEvent += InterstitialAdLoadFailedEvent;
        IronSourceEvents.onInterstitialAdShowSucceededEvent += InterstitialAdShowSucceededEvent;
        IronSourceEvents.onInterstitialAdShowFailedEvent += InterstitialAdShowFailedEvent;
        IronSourceEvents.onInterstitialAdClickedEvent += InterstitialAdClickedEvent;
        IronSourceEvents.onInterstitialAdOpenedEvent += InterstitialAdOpenedEvent;
        IronSourceEvents.onInterstitialAdClosedEvent += InterstitialAdClosedEvent;

        IronSource.Agent.loadInterstitial();
    }


    void OnApplicationPause(bool isPaused)
    {
        IronSource.Agent.onApplicationPause(isPaused);
    }

    #endregion


    #region rewardedVideoMethods

    public void SetRewardVideoCallback(Action callback)
    {
        rewardCallback = callback;
    }

    public void ResetRewardCallback()
    {
        rewardCallback = null;
    }

    private void RewardedVideoAdRewardedEvent(IronSourcePlacement obj)
    {
        if (rewardCallback != null)
        {
            rewardCallback.Invoke();
            ResetRewardCallback();
        }
    }

    private void RewardedVideoAdShowFailedEvent(IronSourceError obj)
    {
        Debug.Log("Playing ad failed");
    }

    private void RewardedVideoAdStartedEvent()
    {
    }

    private void RewardedVideoAdEndedEvent()
    {
    }

    private void RewardedVideoAvailabilityChangedEvent(bool obj)
    {
        rewardVideoAvailable = obj;
    }


    private void RewardedVideoAdOpenedEvent()
    {
    }

    private void RewardedVideoAdClickedEvent(IronSourcePlacement obj)
    {
    }

    private void RewardedVideoAdClosedEvent()
    {
    }

    public void PlayRewardedVideo()
    {
        Debug.Log("Show rewarded video");
        if (rewardVideoAvailable)
            IronSource.Agent.showRewardedVideo();
        else
        {
            RewardedVideoAdShowFailedEvent(null);
        }
    }

    #endregion

    #region InterstitialMethods

    private void InterstitialAdClosedEvent()
    {
        _interstitialDone?.Invoke();
    }

    private void InterstitialAdOpenedEvent()
    {
    }

    private void InterstitialAdClickedEvent()
    {
    }

    private void InterstitialAdShowFailedEvent(IronSourceError obj)
    {
    }

    private void InterstitialAdShowSucceededEvent()
    {
        _interstitialDone?.Invoke();
    }

    private void InterstitialAdLoadFailedEvent(IronSourceError obj)
    {
        Debug.Log($"{obj.getCode()}: {obj.getDescription()}");
    }

    private void InterstitialAdReadyEvent()
    {
        interstitialAvailable = true;
    }

    public void ShowInterstitial(Action done)
    {
        _interstitialDone = done;

        if (interstitialAvailable)
        {
            IronSource.Agent.showInterstitial();
            interstitialAvailable = false;
            IronSource.Agent.loadInterstitial();
            return;
        }

        done?.Invoke();
    }

    #endregion
}