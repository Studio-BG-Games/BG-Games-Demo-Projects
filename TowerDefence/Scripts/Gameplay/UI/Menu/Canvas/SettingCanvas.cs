using System;
using DefaultNamespace;
using DefaultNamespace.Infrastructure.Data;
using Plugins.DIContainer;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Gameplay.UI.Menu.Canvas
{
    public class SettingCanvas : AbsMainMenuCanvas
    {
        [DI(SaveDataProvider.LocalID)] private SaveDataProvider _providerData;
        [DI] private AudioMixerScript _audioMixerScript;
        
        [SerializeField] private Slider _effectSlider;
        [SerializeField] private Slider _musicSlider;
        [SerializeField] private Toggle _blood;
        [SerializeField] private Toggle _vibration;

        private DataSettingGame _dataSetting;
        

        private void OnEnable()
        {
            _dataSetting = _providerData.GetOrDefault<DataSettingGame>();
            _effectSlider.onValueChanged.AddListener(ChangeEffect);
            _musicSlider.onValueChanged.AddListener(ChangeMusic);
            _blood.onValueChanged.AddListener(ChangeBlood);
            _vibration.onValueChanged.AddListener(ChangeVibration);

            _effectSlider.value = _dataSetting.Effect;
            _musicSlider.value = _dataSetting.Volume;
            _blood.isOn = _dataSetting.HasBlood;
            _vibration.isOn = _dataSetting.HasVibration;
        }

        private void ChangeVibration(bool arg0) => _dataSetting.HasVibration = arg0;

        private void ChangeBlood(bool arg0) => _dataSetting.HasBlood = arg0;

        private void ChangeMusic(float arg0)
        {
            _dataSetting.Volume = arg0;
        }

        private void ChangeEffect(float arg0)
        {
            _dataSetting.Effect = arg0;
        }

        private void OnDisable()
        {
            _effectSlider.onValueChanged.RemoveListener(ChangeEffect);
            _musicSlider.onValueChanged.RemoveListener(ChangeMusic);
            _blood.onValueChanged.RemoveListener(ChangeBlood);
            _vibration.onValueChanged.RemoveListener(ChangeVibration);
            _providerData.Save(_dataSetting);
            _audioMixerScript.SetValueToMixer(_dataSetting);
        }
    }
}