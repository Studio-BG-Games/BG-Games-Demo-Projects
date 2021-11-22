using Scripts.Managers;
using Scripts.Serializables.Story;
using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundUpdater : MonoBehaviour
{
    [SerializeField] private BackgroundBase backgroundBase;

    [SerializeField] private Image backgroundImage;
    [SerializeField] private AspectRatioFitter aspectRatioFitter;
    [SerializeField] private Sprite bedroomXPlaceholder;

    [SerializeField] private float shakeDuration = 0.5f;
    [SerializeField] private float shakeStrength = 1f;

    [SerializeField, Range(0f, 0.7f)] private float backgroundOffsetPercent;

    [SerializeField] private float leftPos = 30;
    [SerializeField] private float rightPos = -39;
    [SerializeField] private float centerPos = 0;
    [SerializeField] private float posSwitchTime = 0.3f;

    [SerializeField] private float scrollSpeed = 1f;
    [SerializeField] private Ease easeType = Ease.InOutCubic;

    private readonly string _playerBedroomKey = "bedroomX";
    private bool _isShaking;
    private string _lastBackground = "", _currentBackground = "";
    private float _normalBackgroundPosition;

    public void UpdateBackgroundSprite(Record record)
    {
        Debug.Log("record " + record.background);
        var user = GameManager.Instance.userData;
        var selectedStory = GameManager.Instance.selectedStory;
        var progress = user.progress.Find(value => value.storyId == selectedStory.id);
               
        string backgroundKey = record.background;
        _lastBackground = _currentBackground;

        //check if it is our room
        if (backgroundKey == _playerBedroomKey)
        {
            var backgroundX = GameManager.Instance.scriptableBase.backgroundBase.GetBackgroundByKey(progress.bedroom);
            backgroundImage.sprite = backgroundX == null ? bedroomXPlaceholder : backgroundX.Sprite;
            _currentBackground = backgroundX.name;
        }
        else
        {
            var newBackground =
                GameManager.Instance.scriptableBase.backgroundBase.GetBackgroundByKey(record.background);

            if (record.background != null && record.background.Length > 8)
            {
                var magazine = record.background.Remove(8, record.background.Length - 8);
                var background = record.background.Remove(0,9);
                if (magazine == "magazine")
                {
                    switch (PlayerPrefs.GetString("language"))
                    {
                        case "rus":
                            newBackground = GameManager.Instance.scriptableBase.backgroundBase.GetBackgroundByKey("mag_" + background);
                            break;
                        case "eng":
                            newBackground = GameManager.Instance.scriptableBase.backgroundBase.GetBackgroundByKey("eng_mag_" + background);
                            break;
                    }               
                }
            }

            if (newBackground == null || newBackground.Sprite == null) 
            {
                Debug.LogWarning("Null background " + record.background); 
                return;
            }

            _currentBackground = record.background;
            backgroundImage.sprite = newBackground.Sprite;
        }

        foreach (var background in backgroundBase.Backgrounds)
        {
            if (background.name == record.background)
                backgroundOffsetPercent = background.BackgroundOffsetPercent;
        }

        if (_lastBackground != _currentBackground)
            SpriteScaler.UpdateAspect(backgroundImage.sprite, aspectRatioFitter, backgroundImage.rectTransform, backgroundOffsetPercent);
    }
    
    public void DoBackgroundShake()
    {
        if (_isShaking) return;
        _isShaking = true;
        backgroundImage.transform.DOShakePosition(shakeDuration, shakeStrength).OnComplete(() => _isShaking = false);
    }
    
    public void UpdateSide(string npcKey)
    {
        switch (npcKey)
        {
            case null:
                GoToCenter();
                break;
            case "protagonist":
                GoToLeftSide();
                break;
            default:
                GoToRightSide();
                break;
        }
    }

    private void GoToLeftSide()
    {
        backgroundImage.rectTransform.DOAnchorPosX(leftPos, posSwitchTime);
    }

    private void GoToRightSide()
    {
        backgroundImage.rectTransform.DOAnchorPosX(rightPos, posSwitchTime);
    }

    private void GoToCenter()
    {
        backgroundImage.rectTransform.DOAnchorPosX(centerPos, posSwitchTime);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {           
            DoBackgroundShake();
        }
    }

    public void DoBackgroundScroll(Record record, Action startEvent = null, Action endEvent = null)
    {
        float addBorder = 50f;
        if (record.background == "chriscole")
            addBorder = 600f;

        _normalBackgroundPosition = backgroundImage.rectTransform.anchoredPosition.x;
        startEvent?.Invoke();
        var sequence = DOTween.Sequence();
        var offset = Mathf.Abs(backgroundImage.rectTransform.offsetMin.x);
        sequence.Append(backgroundImage.rectTransform.DOAnchorPosX(_normalBackgroundPosition, scrollSpeed)
           .SetEase(easeType)).SetRelative(false);
        sequence.Append(backgroundImage.rectTransform.DOAnchorPosX(-offset + addBorder, scrollSpeed).SetEase(easeType))
            .SetRelative(true);
        sequence.Append(backgroundImage.rectTransform.DOAnchorPosX(offset - addBorder, scrollSpeed * 2).SetEase(easeType))
            .SetRelative(true);
        sequence.Append(backgroundImage.rectTransform.DOAnchorPosX(_normalBackgroundPosition, scrollSpeed)
            .SetEase(easeType)).SetRelative(false);
        sequence.AppendCallback(() => endEvent?.Invoke());
    }
}