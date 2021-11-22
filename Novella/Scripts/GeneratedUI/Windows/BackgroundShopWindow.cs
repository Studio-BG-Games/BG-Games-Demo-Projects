using System;
using System.Collections.Generic;
using Scripts.Managers;
using Scripts.Serializables.Story;
using Scripts.UISystem;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Scripts;

public class BackgroundShopWindow : WindowController
{
    public event Action<int> OnBackgroundSelected;

    [SerializeField] private GameObject infoPanel;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private TextMeshProUGUI freeButtonTMP, paidButtonTMP;
    private string freeButtonKey = "puton", paidButtonKey = "puton"; 
    [SerializeField] private Image bedroomInfoImage;
    [SerializeField] private GameObject pricePanel, freeButton, paidButton;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image arrow;
    [SerializeField] private Animation movePanel;
    [SerializeField] private Sprite arrowUp, arrowDown;
    [SerializeField] private Sprite freeItemSprite;
    [SerializeField] private Sprite paidItemSprite;

    [SerializeField] private AspectRatioFitter aspectRatioFitter;

    [SerializeField] private float scrollSpeed = 2f;
    [SerializeField] private Ease easeType = Ease.InOutCubic;

    [SerializeField, Range(0f,0.5f)] private float backgroundOffsetPercent = 0.2f;

    private List<Item> _items = new List<Item>();
    private int _currentItemIndex;
    private Item _currentItem;
    private float _normalBackgroundPosition;
    private bool _isHide = false;

    private PlayerInformation playerInformation => GameManager.Instance.playerInformation;


    private void Start()
    {
        _currentItem = _items[_currentItemIndex];
        UpdateBackgroundView();
        _normalBackgroundPosition = backgroundImage.rectTransform.anchoredPosition.x;

        var selectedStory = GameManager.Instance.selectedStory;
        var freeButtonText = selectedStory.general.Find(value => value.system == freeButtonKey);
        var paidButtonText = selectedStory.general.Find(value => value.system == paidButtonKey);
        switch(PlayerPrefs.GetString("language"))
        {
			case "rus":
                freeButtonTMP.text = freeButtonText.russian;
                paidButtonTMP.text = paidButtonText.russian;
                break;
			case "eng":
                freeButtonTMP.text = freeButtonText.english;
                paidButtonTMP.text = paidButtonText.english;
				break;
		}
    }

    public void InitItems(Option[] options)
    {
        foreach (var option in options)
        {
            CreateItem(option);
        }
    }

    private class Item
    {
        public int price;
        public int points;
        public string text;
        public string effect;
        public string itemKey;

        public BackgroundDescription itemDescription { get; }

        public Item(Option option)
        {
            if(PlayerPrefs.GetString("language")=="rus")
                text = option.text;
            else if(PlayerPrefs.GetString("language")=="eng")
                text = option.english;
            price = option.price;
            points = option.points;
            effect = option.effect;
            itemKey = option.item;
            itemDescription = GameManager.Instance.scriptableBase.backgroundBase.GetBackgroundByKey(itemKey);
        }
    }

    public void Next()
    {
        _currentItemIndex++;
        if (_currentItemIndex > _items.Count - 1) _currentItemIndex = 0;
        _currentItem = _items[_currentItemIndex];
        DoBackgroundScroll(() => infoPanel.SetActive(false), () => infoPanel.SetActive(true));
        UpdateBackgroundView();
    }

    public void Back()
    {
        _currentItemIndex--;
        if (_currentItemIndex < 0) _currentItemIndex = _items.Count - 1;
        _currentItem = _items[_currentItemIndex];
        DoBackgroundScroll(() => infoPanel.SetActive(false), () => infoPanel.SetActive(true));
        UpdateBackgroundView();
    }

    public void SetBackgroundRef(Image background)
    {
        backgroundImage = background;
    }
    
    private void UpdateBackgroundView()
    {
        backgroundImage.sprite = _currentItem.itemDescription.Sprite;
        
        SpriteScaler.UpdateAspect(backgroundImage.sprite, aspectRatioFitter, backgroundImage.rectTransform, backgroundOffsetPercent);
        
        descriptionText.SetText(_currentItem.text);
        priceText.SetText(_currentItem.price.ToString());
        
        var isItemAlreadyPurchased = _currentItem.itemDescription.IsItemAlreadyPurchased();
        
        if (_currentItem.price > 0)
        {
            switch (isItemAlreadyPurchased)
            {
                case false:
                    paidButton.SetActive(true);
                    pricePanel.SetActive(true);//закинул как чайлда в paidButton, может и не нужна активация отдельная?
                    freeButton.SetActive(false);
                    break;
                case true:
                    paidButton.SetActive(false);
                    pricePanel.SetActive(false);
                    freeButton.SetActive(true);               
                    return;
            }
        }
        else
        {
            pricePanel.SetActive(false);
            freeButton.SetActive(true);
            paidButton.SetActive(false);
        }
    }

    private void DoBackgroundScroll(Action startEvent = null, Action endEvent = null)
    {
        startEvent?.Invoke();
        var sequence = DOTween.Sequence();
        var offset = Mathf.Abs(backgroundImage.rectTransform.offsetMin.x);
        sequence.Append(backgroundImage.rectTransform.DOAnchorPosX(-offset, scrollSpeed).SetEase(easeType))
            .SetRelative(true);
        sequence.Append(backgroundImage.rectTransform.DOAnchorPosX(offset, scrollSpeed * 2).SetEase(easeType))
            .SetRelative(true);
        sequence.Append(backgroundImage.rectTransform.DOAnchorPosX(_normalBackgroundPosition, scrollSpeed)
            .SetEase(easeType)).SetRelative(false);
        sequence.AppendCallback(() => endEvent?.Invoke());
    }

    public void MovePanel()
    {
        if (!_isHide)
        {
            _isHide = true;
            movePanel.Play("HideShopBG");
            arrow.sprite = arrowUp;
        }
        else
        {
            _isHide = false;
            movePanel.Play("OpenShopBG");
            arrow.sprite = arrowDown;
        }
    }

    public void SaveBackground()
    {
        ref var user = ref GameManager.Instance.userData;
        var selectedStory = GameManager.Instance.selectedStory;
        var progress = user.progress.Find(value => value.storyId == selectedStory.id);
        
        var price = _currentItem.price;
        ref var userCurrency = ref GameManager.Instance.userData.currencies;
        if(progress.paidItems.Find(value => value.Contains(_currentItem.itemKey)) != null)
        {
            progress.bedroom = _currentItem.itemKey;
            OnBackgroundSelected?.Invoke(_currentItemIndex);
            CloseWindow();
            return;
        }
        else if (userCurrency.cash < price)//playerInformation.cash
        {
            WindowsManager.Instance.CreateWindow<NoMoneyPopupWindow>();
            return;
        }

        userCurrency.cash -= price;//playerInformation.cash
        progress.bedroom = _currentItem.itemKey;
        //progress.paidItems.Add(_currentItem.itemDescription.Key);
        _currentItem.itemDescription.BuyItem();//реализовал BuyItem() в BackgroundDescription - верхний коммент не нужен
        FirebaseManager.Instance.UpdateUserData();
        OnBackgroundSelected?.Invoke(_currentItemIndex);
        CloseWindow();
    }

    private void CreateItem(Option option)
    {
        Item item = new Item(option);
        _items.Add(item);
        _currentItem = _items[_currentItemIndex];
        UpdateBackgroundView();
    }

}