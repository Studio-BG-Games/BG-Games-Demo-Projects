using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class EffectsController : MonoBehaviour
{
    [SerializeField] private CanvasGroup blackImageCanvasGroup;
    [SerializeField] private CanvasGroup flashImageCanvasGroup;

    private bool _isPlaying;
    // Start is called before the first frame update

    public void PlayFadeEffect(float transitionTime,
        Action startFadeEvent = null, Action midFadeEvent = null, Action endFadeEvent = null)
    {
        if (_isPlaying) return;
        _isPlaying = true;
        blackImageCanvasGroup.blocksRaycasts = true;
        startFadeEvent?.Invoke();
        Sequence sequence = DOTween.Sequence();
        sequence.Append(blackImageCanvasGroup.DOFade(1f, transitionTime));
        sequence.AppendCallback(() => midFadeEvent?.Invoke());
        sequence.Append(blackImageCanvasGroup.DOFade(0f, transitionTime).OnComplete(() =>
        {
            blackImageCanvasGroup.blocksRaycasts = false;
            _isPlaying = false;
            endFadeEvent?.Invoke();
        }));
    }

    public void PlayFlashEffect(float transitionTime)
    {
        blackImageCanvasGroup.blocksRaycasts = true;
        flashImageCanvasGroup.alpha = 1;
        flashImageCanvasGroup.DOFade(0, transitionTime).OnComplete(() => blackImageCanvasGroup.blocksRaycasts = false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            PlayFlashEffect(1);
        }
    }
}