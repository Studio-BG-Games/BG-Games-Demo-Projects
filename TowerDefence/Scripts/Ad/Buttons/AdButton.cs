using System;
using Gameplay.Waves;
using GoogleMobileAds.Api;
using Infrastructure.ConfigData;
using Plugins.DIContainer;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.Ad.Buttons
{
    public abstract class AdButton : MonoBehaviour
    {
        [SerializeField] protected Button _button;
        [SerializeField] protected CanvasGroup _canvasGroup;
        
        private RewardedAd _ad;

        private void OnEnable()
        {
            _ad = new RewardedAd(GetID());

            OffButton();
            _ad.OnAdLoaded += OnLoad;
            _ad.OnAdFailedToLoad += OnFailLoad;
            _ad.LoadAd(new AdRequest.Builder().Build());

            void OnLoad(object sender, EventArgs e)
            {
                _ad.OnAdOpening += OnOpend;
                _ad.OnAdClosed += OnCloseAd;
                _ad.OnAdFailedToShow += OnFaildShow;
                _ad.OnUserEarnedReward += OnEarnedReward;
                _button.onClick.AddListener(()=>
                {
                    if (_ad.IsLoaded()) _ad.Show();
                });
                OnLoadAbs();
            }
            
        }

        protected  abstract void OnLoadAbs();

        protected abstract void OnFailLoad(object sender, AdFailedToLoadEventArgs e);

        protected abstract string GetID();

        protected abstract void OnEarnedReward(object sender, Reward e);
        

        protected abstract void OnFaildShow(object sender, AdErrorEventArgs e);

        protected abstract void OnCloseAd(object sender, EventArgs e);

        protected abstract void OnOpend(object sender, EventArgs e);

        protected virtual void OffButton()
        {
            _canvasGroup.interactable = _button.interactable = false;
            _button.onClick.RemoveAllListeners();
            _canvasGroup.alpha = 0;
        }

        protected void MakeReobserver()
        {
            _button.onClick.RemoveAllListeners();
            _button.onClick.AddListener(()=>{
                if (_ad.IsLoaded()) _ad.Show();
            });
        }

        protected virtual void OnButton()
        {
            _canvasGroup.interactable =_button.interactable = true;
            _canvasGroup.alpha = 1;
        }
    }
}