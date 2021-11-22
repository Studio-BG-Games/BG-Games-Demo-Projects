using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Analytics;
using Firebase.Extensions;
using System;

public class Analitics : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLevelStart);
    }

    // Update is called once per frame
    private void OnDestroy()
    {
        FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLevelEnd);
    }

    public void OnDeathEvent(int angle, int score)
    {
        int scoreToReport;
        if (score > 100)
            scoreToReport = Convert.ToInt32(Math.Floor(score / 100f) * 100);
        else 
            scoreToReport = score;

        FirebaseAnalytics.LogEvent("Died", new Parameter("figure_angles", angle), new Parameter("score", score), new Parameter("figure","f-"+angle), new Parameter("scoreReport", "score-"+scoreToReport));
        FirebaseAnalytics.LogEvent("DiedAnalitis",new Parameter("figure","f-"+angle), new Parameter("scoreReport", "score-"+scoreToReport));

    }

    public void OnScreenChange(int angle)
    {
        FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventScreenView, new Parameter(FirebaseAnalytics.ParameterScreenClass,"figure"+angle), new Parameter(FirebaseAnalytics.ParameterScreenName, "figure" + angle));        
    }

    public void OnReawrdWatchChoice()
    {
        FirebaseAnalytics.LogEvent("Reawrd video choice");
    }

    public void OnSkipRewardChoice()
    {
        FirebaseAnalytics.LogEvent("Skip video choice");
    }
}
