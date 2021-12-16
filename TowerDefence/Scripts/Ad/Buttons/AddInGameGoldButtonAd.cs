using System;
using DefaultNamespace.Infrastructure.Data;
using Gameplay.GameSceneScript;
using Gameplay.Waves;
using GoogleMobileAds.Api;
using Infrastructure.ConfigData;
using Infrastructure.SceneStates;
using Plugins.DIContainer;
using UnityEngine;

namespace DefaultNamespace.Ad.Buttons
{
    public class AddInGameGoldButtonAd : AdButton
    {
        [DI] private ConfigGame _configGame;
        [DI] private TimeScaler _timeScaler;
        [DI] private GameSceneData _dataScene;
        [DI] private IGold _gold;
        [DI] private IActionFireBase _actionFireBase;

        protected override void OnLoadAbs() => OnButton();

        protected override void OnFailLoad(object sender, AdFailedToLoadEventArgs e) => OffButton();

        protected override string GetID() => _actionFireBase.GetRewardedID("AddInGameGoldButtonAd").GetId();

        protected override void OnEarnedReward(object sender, Reward e)
        {
            _gold.Add(_dataScene.Level.AddingGoldByAd);
            OffButton();
        }

        protected override void OnFaildShow(object sender, AdErrorEventArgs e) => OffButton();

        protected override void OnCloseAd(object sender, EventArgs e) => _timeScaler.Contiun();

        protected override void OnOpend(object sender, EventArgs e) => _timeScaler.Stop();
    }
}