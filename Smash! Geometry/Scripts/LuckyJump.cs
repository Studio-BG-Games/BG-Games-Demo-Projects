using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MEC;
using MoreMountains.NiceVibrations;

public class LuckyJump : MonoBehaviour
{
    [SerializeField] GameObject textObject;
    [SerializeField] GameObject progressBarFillObject;
    [SerializeField] private Player player;
    private Animation animationText;
    private Animation aimatoinTextExplosioin;
    private GameObject currentFigure;
    private GameController gameController;
    private Text text;
    public int times;
    public int shownTimes;

    private void Start()
    {
        player = GetComponent<Player>();
        gameController = GameObject.Find("EventSystem").GetComponent<GameController>();
        text = textObject.GetComponent<Text>();
        animationText = textObject.GetComponent<Animation>();
        aimatoinTextExplosioin = textObject.transform.GetChild(0).GetComponent<Animation>();
        times = 0;
        shownTimes = 2;
        gameController.OnDeath+=OnDeath;
    }

    public void SetLuckyJump()
    {
        times++;
        StartCoroutine(WaiteToChangeColor());
        if (!gameController.menu)
            DoLuckyJumpImpact();
        if(times == shownTimes && !gameController.menu && shownTimes<=9)
        {
            times = 0;
            shownTimes++;
            currentFigure = gameController._currentBorder;
            gameController.scoreMuliplier++;
            StartCoroutine(OutputLuckyJump());
        }

    }

    public void SetTimesToZero()
    {
        
        times = 0;
        if (gameController.scoreMuliplier > 1 && LuckyJumpAnim.animInstanse!=null)
        {
            shownTimes = 2;
            player.SetBackgroundLight(false);
            LuckyJumpAnim.animInstanse.GetComponent<LuckyJumpAnim>().increaseOnFail = true;
        }
        gameController.scoreMuliplier = 1;
        textObject.SetActive(false);
    }
    private void DoLuckyJumpImpact()
    {
        if (!gameController._vibrMute)
            MMVibrationManager.Haptic(HapticTypes.LightImpact);
    }
    private IEnumerator OutputLuckyJump()
    {
        StopCoroutine("WaitToDisableObject");
        yield return new WaitForSeconds(0.3f);
        if (!gameController._death)
        {
            textObject.SetActive(true);
            textObject.transform.GetChild(0).gameObject.SetActive(true);
            StartCoroutine(WaitToDisableObject(textObject.transform.GetChild(0).gameObject, 0.51f));
            aimatoinTextExplosioin.gameObject.GetComponent<Text>().text = gameController.scoreMuliplier + "x";
            text.text = gameController.scoreMuliplier + "x";
            animationText.Play("LuckyJump");
            aimatoinTextExplosioin.Play("LuckyJumpExp");
            player.SetBackgroundLight(true);
        }
    }

    private IEnumerator WaiteToChangeColor()
    {
        yield return new WaitForSeconds(0.3f);
        text.color = progressBarFillObject.GetComponent<Image>().color;
        yield break;
    }
    private IEnumerator WaitToDisableObject(GameObject gameObject, float sec)
    {
        yield return new WaitForSeconds(sec);
        gameObject.SetActive(false);
    }

    
    private void OnDeath()
    {
        player.SetBackgroundLight(false);
        times = 0;
        shownTimes = 2;
        textObject.SetActive(false);
    }
    
}
