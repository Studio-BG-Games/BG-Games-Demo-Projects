using System;
using System.Runtime.InteropServices;
using GoogleMobileAds.Api;
using Newtonsoft.Json;
using UnityEngine;

namespace DefaultNamespace.Ad
{
    [System.Serializable]
    public class AdIdRewarded
    {
        [JsonIgnore]public string Name => _name;
        [JsonProperty][SerializeField] private string _name;
        [JsonProperty][SerializeField] protected string _androindId = "ca-app-pub-3940256099942544/5224354917";
        [JsonProperty][SerializeField] protected string _iosID = "ca-app-pub-3940256099942544/1712485313";

        public AdIdRewarded(string name) => _name = name;

        public string GetId()
        {
            if((string.IsNullOrWhiteSpace(_androindId) || string.IsNullOrWhiteSpace(_iosID)))
                Debug.LogError($"{_name} не имет API ID для рекламы");
            if(_androindId == "ca-app-pub-3940256099942544/5224354917" || _iosID == "ca-app-pub-3940256099942544/1712485313")
                Debug.LogError($"у {_name} стоят тестовые ID");
            if (Application.isEditor)
                return "ca-app-pub-3940256099942544/5224354917"; // тестовый ID - https://developers.google.com/admob/unity/test-ads#android
            else if(Application.platform == RuntimePlatform.Android)
                return _androindId;
            else
                return _iosID;
        }
    }
}