using System.Configuration;
using DefaultNamespace.Infrastructure.Data;
using Plugins.DIContainer;
using UnityEngine;
using UnityEngine.Audio;

namespace DefaultNamespace
{
    public class AudioMixerScript
    {
        [DI] private AudioMixer _audioMixer;

        public void SetValueToMixer(DataSettingGame dataSettingGame)
        {
            _audioMixer.SetFloat("Effect", dataSettingGame.Effect);
            _audioMixer.SetFloat("Music", dataSettingGame.Volume);
        }
    }
}