using Scripts.Managers;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;
using System.Threading;
using Scripts.UISystem;
using GeneratedUI;

public class TimerForAds : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI time;
    [SerializeField] GameObject timer;
    private DateTime globalTime;

    private void Start()
    {
        globalTime = CheckGlobalTime();

        if (PlayerPrefs.GetInt("TimerForAdsIsStarted") == 1)
            timer.SetActive(true);
    }

    private void OnDestroy()
    {
        SDKManager.Instance.ResetRewardCallback();
    }

    void Update()
    {
        if (PlayerPrefs.GetInt("TimerForAdsIsStarted") == 1)
        {
            var closeTime = Convert.ToDateTime(PlayerPrefs.GetString("CloseTimeOfTimerForAds"));
            if (globalTime > closeTime)
            {
                PlayerPrefs.SetInt("TimerForAdsIsStarted", 0);
                timer.SetActive(false);
                return;
            }

            globalTime = globalTime.AddSeconds(0.02f);
            var currentTime = globalTime.TimeOfDay;

            if (closeTime.TimeOfDay > currentTime)
            {
                time.text = (closeTime.TimeOfDay - currentTime).ToString().Substring(0, 8);
            }
            else if (closeTime.TimeOfDay <= currentTime)
            {
                PlayerPrefs.SetInt("TimerForAdsIsStarted", 0);
                timer.SetActive(false);
            }
        }
    }

    public void StartTimer()
    {
        Debug.Log("Rewarded video available : " + SDKManager.Instance.RewardVideoAvailable);
        if (PlayerPrefs.GetInt("TimerForAdsIsStarted") == 0 && SDKManager.Instance.RewardVideoAvailable)
        {
            globalTime = CheckGlobalTime();
            PlayerPrefs.SetInt("TimerForAdsIsStarted", 1);
            PlayerPrefs.SetString("CloseTimeOfTimerForAds", globalTime.AddHours(1).ToString());

            //Place for some logic to show ads
            SDKManager.Instance.PlayRewardedVideo();
            timer.SetActive(true);
        }
    }

    private void OnApplicationFocus(bool focus)
    {
        if (focus) globalTime = CheckGlobalTime();
    }

    private DateTime CheckGlobalTime()
    {
        var www = new WWW("https://google.com");
        while (!www.isDone && www.error == null)
            Thread.Sleep(1);

        var str = www.responseHeaders["Date"];
        DateTime dateTime;
        if (!DateTime.TryParse(str, out dateTime))
            return DateTime.MinValue;

        return dateTime.ToUniversalTime();
    }
}