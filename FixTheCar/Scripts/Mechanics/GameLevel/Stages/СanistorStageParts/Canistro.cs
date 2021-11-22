using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Mechanics.GameLevel.Stages.СanistorStageParts
{
    public class Canistro : MonoBehaviour, IPointerClickHandler
    {
        public event Action StartCaistroAction;
        public event Action FinishCanistroAction;
        
        public SizeElement Size => _sizeElement;
        [SerializeField] private SizeElement _sizeElement;
        [SerializeField] private Image _fueilImage;
        [SerializeField] private SelectorImageByState _selectorImage;

        private FactoryStateCanistro _factoryState;
        private Dictionary<Type, CanistroState> _states = new Dictionary<Type, CanistroState>();
        private CanistroState _currenstState;

        private void Awake() => Hide(0.00001f);

        public void Init(FactoryStateCanistro factoryCanistro) => _factoryState = factoryCanistro;

        public void Hide(float durationFade, Action callback=null) => _fueilImage.DOFade(0, durationFade).OnComplete(()=>callback?.Invoke());

        public void Show(float durationUnfade, Action callback=null)
            => _fueilImage.DOFade(1, durationUnfade).OnComplete(()=>callback?.Invoke());

        public void ChangeState<State>() where State : CanistroState
        {
            if (_states.TryGetValue(typeof(State), out var result))
                _currenstState = result;
            else
                AddToDict(_currenstState = _factoryState.GetNewState<State>(this));
            _selectorImage.SelectImageBy(_currenstState);
        }

        private void AddToDict(CanistroState state) => _states.Add(state.GetType(), state);

        public void ChangeValueFuel(int changeTo, float _duration, Action callback = null)
        {
            if(changeTo!= 1 && changeTo!=0) throw new Exception("Invalid number");
            _fueilImage.DOFillAmount(changeTo, _duration).OnComplete(()=>callback?.Invoke());
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            StartCaistroAction?.Invoke();
            _currenstState.Action(()=>FinishCanistroAction?.Invoke());
        }
    }
}