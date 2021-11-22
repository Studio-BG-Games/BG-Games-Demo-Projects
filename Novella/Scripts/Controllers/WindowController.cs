using UnityEngine;
using DG.Tweening;
using  Scripts.Managers;
using Scripts.UISystem;

namespace Scripts
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class WindowController : MonoBehaviour
    {
        public event System.Action onWindowClose;
        public event System.Action onWindowOpen;
        public event System.Action onWindowCompleteDealEvent;

        protected CanvasGroup canvasGroup;
        protected RectTransform rectTransform;

        public bool IsOnTop => WindowsManager.Instance.IsWindowOnTop(this);

        public enum TransitionType
        {
            None,
            Fade,
            TransitionBottom,
            TransitionTop,
            TransitionLeft,
            TransitionRight,
        }


        [Header("Window Show/Hide Animations")]
        public float animationTime = 1f;

        public TransitionType transitionType1;
        public TransitionType transitionType2;
        public bool changeCanvasGroup = true;

        private const UpdateType UpdateType = DG.Tweening.UpdateType.Normal;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            rectTransform = GetComponent<RectTransform>();
        }

        [ContextMenu("Open")]
        public virtual void OpenWindow()
        {
            ShowAnimation(transitionType1);
            ShowAnimation(transitionType2);

            if (changeCanvasGroup)
            {
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
            }

            UpdateView();
        }


        [ContextMenu("Close")]
        public virtual void CloseWindow()
        {
            HideAnimation(transitionType1);
            HideAnimation(transitionType2, true);

            if (changeCanvasGroup)
            {
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
            }

            onWindowClose?.Invoke();
        }

        public void ShowAnimation(TransitionType transitionType)
        {
            switch (transitionType)
            {
                case TransitionType.None:
                    canvasGroup.alpha = 1f;
                    break;

                case TransitionType.Fade:
                    canvasGroup.alpha = 0f;
                    canvasGroup.DOFade(1f, animationTime).SetUpdate(UpdateType, true);
                    break;

                case TransitionType.TransitionBottom:
                    rectTransform.anchoredPosition = new Vector2(0f, -rectTransform.rect.size.y);
                    rectTransform.DOAnchorPosY(0f, animationTime).SetUpdate(UpdateType, true);
                    break;

                case TransitionType.TransitionTop:
                    rectTransform.anchoredPosition = new Vector2(0f, rectTransform.rect.size.y);
                    rectTransform.DOAnchorPosY(0f, animationTime).SetUpdate(UpdateType, true);
                    break;

                case TransitionType.TransitionLeft:
                    rectTransform.anchoredPosition = new Vector2(-rectTransform.rect.size.x, 0f);
                    rectTransform.DOAnchorPosX(0f, animationTime).SetUpdate(UpdateType, true);
                    break;

                case TransitionType.TransitionRight:
                    rectTransform.anchoredPosition = new Vector2(rectTransform.rect.size.x, 0f);
                    rectTransform.DOAnchorPosX(0f, animationTime).SetUpdate(UpdateType, true);
                    break;
            }
            

            var sequence = DOTween.Sequence();
            sequence.SetUpdate(UpdateType, true);
            sequence.AppendInterval(animationTime);
            sequence.OnComplete(() => onWindowOpen?.Invoke());
        }

        public void HideAnimation(TransitionType transitionType, bool destroyOnClose = false)
        {
            switch (transitionType)
            {
                case TransitionType.None:
                    canvasGroup.alpha = 0f;
                    break;

                case TransitionType.Fade:
                    canvasGroup.alpha = 1f;
                    canvasGroup.DOFade(0f, animationTime).SetUpdate(UpdateType, true);
                    break;

                case TransitionType.TransitionBottom:
                    rectTransform.DOAnchorPosY(-rectTransform.rect.size.y, animationTime).SetUpdate(UpdateType, true);
                    break;

                case TransitionType.TransitionTop:
                    rectTransform.DOAnchorPosY(rectTransform.rect.size.y, animationTime).SetUpdate(UpdateType, true);
                    break;

                case TransitionType.TransitionLeft:
                    rectTransform.DOAnchorPosX(-rectTransform.rect.size.x, animationTime).SetUpdate(UpdateType, true);
                    break;

                case TransitionType.TransitionRight:
                    rectTransform.DOAnchorPosX(rectTransform.rect.size.x, animationTime).SetUpdate(UpdateType, true);
                    break;
            }

            if (!destroyOnClose) return;

            var sequence = DOTween.Sequence();
            sequence.SetUpdate(UpdateType, true);
            sequence.AppendInterval(animationTime);
            sequence.OnComplete(() => Destroy(gameObject));
        }

        public virtual void UpdateView()
        {
        }

        public void WindowCompleteDealEvent()
        {
            onWindowCompleteDealEvent?.Invoke();
        }
    }
}