using Scripts.Managers;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;
using System.Threading;

public class TimerForCocktail : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI time;
    [SerializeField] TextMeshProUGUI cocktails;
    [SerializeField] GameObject timer;
    private DateTime globalTime;
    private bool isDataLoaded;
    private GameManager _gameManager;

    private void Start()
    {
        globalTime = CheckGlobalTime();
        _gameManager = GameManager.Instance;
        _gameManager.OnLoadUserDataComplete += ChangeDataLoadingStatus;
    }

    private void ChangeDataLoadingStatus()
    {
        isDataLoaded = true;
    }
    void Update()
    {
        if(isDataLoaded)
        {
            ref var user = ref GameManager.Instance.userData;
            if (user.currencies.cocktails < 2)
            {
                if (PlayerPrefs.GetInt("TimerIsStarted") == 0)
                {
                    PlayerPrefs.SetInt("TimerIsStarted", 1);
                    PlayerPrefs.SetString("CloseTimeOfTimer", globalTime.AddHours(2).ToString());
                }
                else
                {           
                    var closeTime = Convert.ToDateTime(PlayerPrefs.GetString("CloseTimeOfTimer"));

                    if(globalTime >= closeTime)
                    {
                        PlayerPrefs.SetInt("TimerIsStarted", 0);

                        if (user.currencies.cocktails + 1 == 2)
                        { 
                            user.currencies.cocktails += 1;
                            cocktails.text = user.currencies.cocktails.ToString();
                        }
                        else
                        {
                            user.currencies.cocktails += 1;
                            PlayerPrefs.SetInt("TimerIsStarted", 1);
                            PlayerPrefs.SetString("CloseTimeOfTimer", closeTime.AddHours(2).ToString());
                        }
                        FirebaseManager.Instance.UpdateUserCurrencies();
                        //ES3.Save("user", GameManager.Instance.userData);
                        timer.SetActive(false);       
                        return;
                    }

                    if (timer.activeSelf != true)
                        timer.SetActive(true);

                    globalTime = globalTime.AddSeconds(0.02f);
                    var currentTime = globalTime.TimeOfDay;

                    if (closeTime.TimeOfDay > currentTime)
                    {
                        time.text = (closeTime.TimeOfDay - currentTime).ToString().Substring(0, 8);
                    }
                    else if (closeTime.TimeOfDay <= currentTime)
                    {
                        PlayerPrefs.SetInt("TimerIsStarted", 0);
                        user.currencies.cocktails += 1;
                        FirebaseManager.Instance.UpdateUserCurrencies();
                        //ES3.Save("user", GameManager.Instance.userData);
                        timer.SetActive(false);
                        cocktails.text = user.currencies.cocktails.ToString();
                    }
                }

            }
            else
            { 
                timer.SetActive(false);
            }
        }
    }

    private void OnApplicationFocus(bool focus)
    {
        if(focus) globalTime = CheckGlobalTime();
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
        private void OnDestroy()
    {
        _gameManager.OnLoadUserDataComplete -= ChangeDataLoadingStatus;
    }
}
