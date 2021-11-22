/*using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;

public class Leaderboard : MonoBehaviour
{
    private const string leaderBoardID = "CgkI0sXtiPcPEAIQAQ";
    void Awake()
    {
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
       // enables saving game progress.
       .EnableSavedGames()
       // requests the email address of the player be available.
       // Will bring up a prompt for consent.
       .RequestEmail()
       // requests a server auth code be generated so it can be passed to an
       //  associated back end server application and exchanged for an OAuth token.
       .RequestServerAuthCode(false)
       // requests an ID token be generated.  This OAuth token can be used to
       //  identify the player to other services such as Firebase.
       .RequestIdToken()
       .Build();

        PlayGamesPlatform.InitializeInstance(config);
        // recommended for debugging:
        PlayGamesPlatform.DebugLogEnabled = true;
        // Activate the Google Play Games platform
        PlayGamesPlatform.Activate();
    }

    private void Start()
    {
        PlayGamesPlatform.Instance.Authenticate(SignInInteractivity.CanPromptOnce, (result) =>
        {
            // handle results
        });
    }

    public void ReportScore(int score)
    {
        Social.ReportScore(score, leaderBoardID, (bool success) =>
        {
        });
    }

    public void AchiveClick()
    {
        Social.ShowLeaderboardUI();
        Debug.Log("Click");
    }

    private void OnApplicationQuit()
    {
        PlayGamesPlatform.Instance.SignOut();
    }
}*/
