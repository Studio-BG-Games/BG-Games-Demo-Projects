using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace Gameplay.UI.Huds
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class HudElement : MonoBehaviour
    {
        [Min(0)][SerializeField] private float _fadeTimeOn;
        [Min(0)][SerializeField] private float _fadeTimeOff;
        
        private CanvasGroup _canvasGroup;
        private Tween _fadeTween;
        private Coroutine _awaitBeforeAction;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            CustomAwake();
        }

        public void FadeToInTime(float actionIn, float timeFade, ChangeStateToHudElement turnOn)
        {
            TryStopCorutine();
            TryStopTween();
            _awaitBeforeAction = StartCoroutine(FadeWithTimer(actionIn, timeFade, turnOn));
        }

        public void FadeToInTime(float actionIn, ChangeStateToHudElement turnOn) => FadeToInTime(actionIn, GetDefaultTimeFade(turnOn), turnOn);

        public void FadeTo(float timeToFade, ChangeStateToHudElement turnOn)
        {
            TryStopTween();
            TryStopCorutine();
            _fadeTween = _canvasGroup.DOFade(GetAlpha(turnOn), timeToFade);
        }

        public void FadeTo(ChangeStateToHudElement turnOn) => FadeTo(GetDefaultTimeFade(turnOn), turnOn);

        private void TryStopCorutine()
        {
            if(_awaitBeforeAction==null)
                return;
            StopCoroutine(_awaitBeforeAction);
        }

        private IEnumerator FadeWithTimer(float timeAwait, float timeFade, ChangeStateToHudElement toHudElement)
        {
            yield return new WaitForSeconds(timeAwait);
            FadeTo(timeFade, toHudElement);
        }

        private void TryStopTween()
        {
            if(_fadeTween==null)
                return;
            _fadeTween.Complete();
            _fadeTween = null;
        }

        private float GetAlpha(ChangeStateToHudElement state) => state == ChangeStateToHudElement.TurnOn ? 1 : 0;

        private float GetDefaultTimeFade(ChangeStateToHudElement state) => state == ChangeStateToHudElement.TurnOn ? _fadeTimeOn : _fadeTimeOff;
        
        

        protected virtual void CustomAwake(){}
    }
    public enum ChangeStateToHudElement { TurnOn, TurnOff }
}