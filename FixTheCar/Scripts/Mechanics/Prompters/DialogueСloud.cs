using System;
using System.Collections;
using DG.Tweening;
using Infrastructure.Configs;
using Plugins.DIContainer;
using Plugins.Interfaces;
using TMPro;
using UnityEngine;

namespace Mechanics.Prompters
{
    public class DialogueСloud : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _groupCloud;
        [SerializeField] private TextMeshProUGUI _label;
        [Min(0)] [SerializeField] private float _durationCanvas;

        [DI] private ConfigGame _configGame;
        
        private Tween _tweenOpenClose;
        private Coroutine _textUpdate;
        
        public void Say(string mes, Action callback = null)
        {
            Clear();
            if(_groupCloud.alpha<1)
                Open(()=>StartTextUpdate(mes, callback));
            else
                StartTextUpdate(mes,callback);
        }

        private void OnDisable() => Clear();

        public void Clear() => _label.text = string.Empty;

        public void Close(Action oncomplete = null)
        {
            StopWork();
            _tweenOpenClose = _groupCloud.DOFade(0, _durationCanvas).OnComplete(()=>oncomplete?.Invoke());
        }

        public void CloseMoment()
        {
            StopWork();
            _groupCloud.alpha = 0;
        }

        private void StartTextUpdate(string mes, Action callback)
        {
            StopTextUpdate();
            if (enabled)
                _textUpdate = StartCoroutine(MakeText(mes, _configGame.SpeedCurveTextPrompter.Evaluate(mes.Length), callback));
        }

        private void StopTextUpdate()
        {
            if(_textUpdate!=null)
                StopCoroutine(_textUpdate);
        }

        private void Open(Action onComplete = null)
        {
            StopWork();
            _tweenOpenClose = _groupCloud.DOFade(1, _durationCanvas).OnComplete(() => onComplete?.Invoke());
        }

        private void StopWork()
        {
            if (_tweenOpenClose != null)
                _tweenOpenClose.Kill();
            StopTextUpdate();
            Clear();
        }

        private IEnumerator MakeText(string mes, float duration, Action callback)
        {
            for (int i = 0; i < mes.Length; i++)
            {
                _label.text += mes[i];
                yield return new WaitForSeconds(duration);
            }
            callback?.Invoke();
        }
    }
}