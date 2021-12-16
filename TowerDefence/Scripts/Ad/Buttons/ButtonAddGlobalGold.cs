using System;
using DefaultNamespace.Infrastructure.Data;
using Gameplay.UI.Menu;
using GoogleMobileAds.Api;
using Plugins.DIContainer;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace DefaultNamespace.Ad.Buttons
{
    public class ButtonAddGlobalGold : AdButton
    {
        [SerializeField] private string _nameApiIdAd;
        [SerializeField] private TextMeshProUGUI _labelCostAd;
        [SerializeField] private float _alphaOff;
        [SerializeField] private UnityEvent Awarded;
        [SerializeField] private UnityEvent NotAwarded;

        [DI] private MainGold _mainGold;
        [DI] private TimeScaler _timeScaler;
        [DI] private IActionFireBase _actionFireBase;
        
        
        private AdIdRewardedWithNameAndCount _adSetting;


        protected override void OnLoadAbs()
        {
            OnButton();   
        }

        protected override void OnFailLoad(object sender, AdFailedToLoadEventArgs e) => OffButton();

        protected override string GetID()
        {
            var idAD = _actionFireBase.GetExtendedRewardedId(_nameApiIdAd);
            _adSetting = idAD;
            Debug.Log(_adSetting);
            _labelCostAd.text = idAD.Count.ToString();
            return idAD.GetId();
        }

        protected override void OnEarnedReward(object sender, Reward e)
        {
            _mainGold.Add(_adSetting.Count);
            Awarded?.Invoke();
            OffButton();
        }

        protected override void OnFaildShow(object sender, AdErrorEventArgs e)
        {
            NotAwarded?.Invoke();
            OffButton();
        }

        protected override void OnCloseAd(object sender, EventArgs e)
        {
            OffButton();
            _timeScaler.Contiun();
        }

        protected override void OnOpend(object sender, EventArgs e)
        {
            _timeScaler.Stop();
        }

        protected override void OnButton()
        {
            _canvasGroup.interactable = true;
            _canvasGroup.alpha = 1;
        }

        protected override void OffButton()
        {
            _canvasGroup.interactable = false;
            _canvasGroup.alpha = 0.3f;
        }
    }
}