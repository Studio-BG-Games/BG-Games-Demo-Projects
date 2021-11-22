using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MEC;
using Firebase;
using Firebase.Analytics;
using Firebase.Extensions;

public class ReviveGame : MonoBehaviour
{
    [SerializeField] private Button continueGame;
    [SerializeField] private Button skip;
    [SerializeField] private GameController gameController;
    [SerializeField] private Text highScore;
    [SerializeField] private Text score;
    [SerializeField] private GameObject continueObject;
    [SerializeField] private Analitics analitics;
    public static GameObject reviveGameInstanse;


    void Awake()
    {
        if (reviveGameInstanse == null)
        {
            reviveGameInstanse = this.gameObject;
        }
        else
        {
            Destroy(gameObject);
        }
        StartCoroutine(DoTimer());
        gameController = GameObject.Find("EventSystem").GetComponent<GameController>();
        gameController._currentBorder.gameObject.SetActive(false);
            
        //continueGame = GameObject.Find("UI").transform.GetChild(0).GetComponent<Button>();
        //skip = GameObject.Find("UI").transform.GetChild(1).GetComponent<Button>();
        
        continueGame.onClick.AddListener(ContinueGame);
        skip.onClick.AddListener(Skip);
        //gameObject.transform.SetParent(GameObject.Find("UI").transform);
        score.text = ""+gameController._currentScore;
        highScore.text = "High Score: " + gameController._record;
        analitics = GameObject.Find("EventSystem").GetComponent<Analitics>();
        // gameObject.GetComponent<RectTransform>().position = new Vector3(0, 0, gameObject.GetComponent<RectTransform>().position.z);
    }


    private void ContinueGame()
    {
//#if UNITY_EDITOR
        gameController.ContinueGame();
        Destroy(gameObject);
/*/#else
        if (!gameController.failetoLoadRewardedAd && gameController.interAdd.rewardLoaded == true)
        {
            continueGame.interactable = false;
            gameController.interAdd.ShowRewardedAd();
            OnReawrdWatchChoice();
            continueGame.gameObject.SetActive(false);
            gameController._currentBorder.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            gameController.interAdd.rewardRequested = true;
        }
#endif      */ 
    }

    private void Skip()
    {
        OnSkipRewardChoice();
        gameController.CheckOnDeathAd();
        Timing.RunCoroutine(gameController.RestartGame());
        Destroy(gameObject);
    }

    IEnumerator DoTimer()
    {
        for(int i = 5; i > 0; i--)
        {
            continueObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Revive (" + i + ")";
            yield return new WaitForSeconds(1);

        }
        gameObject.SetActive(false);
        continueGame.interactable = false;
        gameController.CheckOnDeathAd();
        Timing.RunCoroutine(gameController.RestartGame());
        yield break;
    }

    public void OnReawrdWatchChoice()
    {
        FirebaseAnalytics.LogEvent("reawrd_video_choice");
    }

    public void OnSkipRewardChoice()
    {
        FirebaseAnalytics.LogEvent("skip_video_choice");
    }
}
