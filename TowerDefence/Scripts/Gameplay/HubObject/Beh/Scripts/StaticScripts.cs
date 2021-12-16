using Gameplay.Builds;
using Plugins.DIContainer;
using Plugins.HabObject;
using Plugins.Sound;
using Plugins.Sound.Sound2DLoops;
using Plugins.Sound.Sound2Ds;
using UnityEngine;

namespace Gameplay.HubObject.Beh.Scripts
{
    public static class StaticScripts
    {
        public static SoundSystem SoundStstem => _soundStstem!=null ? _soundStstem : _soundStstem = DiBox.MainBox.ResolveSingle<SoundSystem>();
        private static SoundSystem _soundStstem;
        
        public static void DestroyObject(HabObject parent) => Object.Destroy(parent.gameObject);
        
        public static void DestroyObject(HabObject parent, float delay) => Object.Destroy(parent.gameObject, delay);
        
        public static bool IsPlayAnimation(AnimatorApplayCallback applayCallback, string nameAnimation) => applayCallback.IsCurrentAnimation(nameAnimation, 0);
        
        public static void Play(AnimatorApplayCallback applayCallback, string nameAnimation) => applayCallback.StartAnimation(nameAnimation);

        public static void MakeParticleSystemByVector(ParticleSystem particleSystem, Vector3 position) =>
            Object.Instantiate(particleSystem, position, Quaternion.identity);

        public static void TurnToComponent(Behaviour component, bool turnTo) => component.enabled = turnTo;

        public static void MakeParticleSystemByTransform(ParticleSystem particleSystem, Transform transform, bool parented)
        {
            if(parented) Object.Instantiate(particleSystem, transform.position, Quaternion.identity, transform);
            else MakeParticleSystemByVector(particleSystem, transform.position);
        }

        public static bool IsEqulis<T>(T target, T conditionWith) => Equals(target, conditionWith);

        public static void PlaySound(ISound2D sound, Transform point)
        {
            var source = SoundStstem.Play(sound);
            source.transform.position = point.transform.position;
        }
        
        public static void StartLoopSound(ISound2DLoop sound, Transform point)
        {
            SoundStstem.Play(sound, SoundSystem.LoopAction.Start, out var result);
            result.transform.position = point.transform.position;
        }

        public static void StopLoopSound(ISound2DLoop sound) => SoundStstem.Play(sound, SoundSystem.LoopAction.Stop, out var t);

        public static void ReportYandexEvent(string name) => AppMetrica.Instance.ReportEvent(name);

        public static void ReportYandexEvent(string name, string json) => AppMetrica.Instance.ReportEvent(name, json);
    }
}