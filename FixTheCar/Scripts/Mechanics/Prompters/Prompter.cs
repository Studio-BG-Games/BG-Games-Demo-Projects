using System;
using DG.Tweening;
using Mechanics.Interfaces;
using UnityEngine;

namespace Mechanics.Prompters
{
    public class Prompter : AbsPrompter
    {
        [Header("Настройки круга")]
        [SerializeField] private CanvasGroup _groupCircle;
        [SerializeField] [Min(0)] private float _durationCircle;
        
        [Header("Настройки движения мыши")]
        [SerializeField] private Transform _downPoint;
        [SerializeField] private Transform _upPoint;
        [SerializeField] private Transform _mouse;
        [SerializeField] private float _durationMouse;

        [Header("Диалоговое окно")] 
        [SerializeField] private DialogueСloud _dialogueСloud;
        
        private void Awake()
        {
            _mouse.position = _downPoint.position;
            _groupCircle.alpha = 0;
            _dialogueСloud.CloseMoment();
        }

        public override void Unhide(Action action = null)
        {
            _groupCircle.DOFade(1, _durationCircle).
                OnComplete(
                () => _mouse.DOMove(_upPoint.position, _durationMouse).
                    OnComplete(
                        () => action?.Invoke()
                        )
                );
        }

        
        
        public override void Say(string message, Action callback = null) => _dialogueСloud.Say(message, callback);

        public override void Hide(Action action = null)
        {
            _dialogueСloud.Close();
            _mouse.DOMove(_downPoint.position, _durationMouse).
                OnComplete(
                    () => _groupCircle.DOFade(0, _durationCircle).
                        OnComplete(
                            ()=>action?.Invoke()
                            )
                    );
        }

        public void Init(Prompter prev)
        {
            _groupCircle.alpha = prev._groupCircle.alpha > 0.1f ? 1 : 0;
            _mouse.position = prev._groupCircle.alpha > 0.1f ? _upPoint.position : _downPoint.position;
        }

        #region Unity_Visual

        [ContextMenu("To Up")]
        private void MoveToUp() => _mouse.position = _upPoint.position;
        
        [ContextMenu("To Down")]
        private void MoveToDown() => _mouse.position = _downPoint.position;

        private void OnDrawGizmos()
        {
            if(!_downPoint || !_upPoint || !_mouse)
                return;
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_upPoint.position, 25);
            Gizmos.DrawWireSphere(_downPoint.position, 25);
            Gizmos.color = Color.green;
            Gizmos.DrawLine(_downPoint.position, _upPoint.position);
        }

        #endregion

       
    }
}