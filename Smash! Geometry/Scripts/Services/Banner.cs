using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using UnityEngine.UI;

public class Banner : MonoBehaviour
{
    [SerializeField] private GameController gameController;

    private BannerView adBanner;
    private string bannerID = "ca-app-pub-3940256099942544/6300978111"; //fake
   // private string bannerID = "ca-app-pub-7635126548465920/4928024599"; //android
   // private string bannerID = "ca-app-pub-7635126548465920/8920429866"; //ios
    public GameObject saveVariables;
    public bool isLoaded;
    private static GameObject bannerInstanse;
    void Awake()
    {
        DontDestroyOnLoad(this);

        if (bannerInstanse == null)
        {
            bannerInstanse = this.gameObject;
        }
        else
        {
            Destroy(gameObject);
        }
    
        gameController = GameObject.Find("EventSystem").GetComponent<GameController>();
       
    }

    public void RequestBunnerAd()
    {
        adBanner = new BannerView(bannerID, AdSize.Banner, AdPosition.Bottom);
        AdRequest request = AdRequestBuild();
        adBanner.LoadAd(request);
        adBanner.OnAdLoaded += AdBanner_OnAdLoaded;
    }

    private void AdBanner_OnAdLoaded(object sender, System.EventArgs e)
    {
        isLoaded = true;
        Debug.Log("loaded");
    }

    AdRequest AdRequestBuild()
    {
        return new AdRequest.Builder().Build();
    }

    public void HideBanner()
    {
        adBanner.Hide();
    }

    public void ShowBanner()
    {
        adBanner.Show();
    }
}
