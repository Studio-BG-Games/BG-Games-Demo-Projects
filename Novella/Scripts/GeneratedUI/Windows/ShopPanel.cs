using System;
using System.Collections.Generic;
using System.Linq;
using Scripts.Managers;
using Scripts.Serializables.Story;
using Scripts.Serializables.User;
using Scripts.UISystem;
using DG.Tweening;
using GeneratedUI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ShopPanel : MonoBehaviour
{
    public event Action<int> OnShopComplete;
    public event Action<int, int> OnShopCompleteBestItem;

    private DressingRoomWindow _dressingRoomWindow;
    private ItemDescription CurrentItemDescription => _dressingRoomWindow.currentItemContainer.currentItem;
    private Item CurrentItem => _items[ItemDescriptions.IndexOf(CurrentItemDescription)];

    private int _selectedItemIndex = -1;//важно

    [SerializeField] private Button selectItemButton;
    [SerializeField] private Button buyItemButton;
    [SerializeField] private Button freeItemButton;
    [SerializeField] private Button continueGameButton;
    [SerializeField] private TextMeshProUGUI paidItemPriceText;

    private List<Item> _items = new List<Item>();
    public List<ItemDescription> ItemDescriptions { get; } = new List<ItemDescription>();

    private bool _isPlayerContinueShopping;
    private bool _isPlayerStopShopping;    
    private List<Item> _boughtItems = new List<Item>();

    private PlayerInformation playerInformation => GameManager.Instance.playerInformation;

    public ItemType ShopItemType => _items.First().itemType;

    public enum ItemType
    {
        Hair,
        Dress
    }

    private class Item
    {
        public int price;
        public int points;
        public string text;
        public string effect;
        public string itemKey;

        public int index;

        // public bool isUnlock => itemDescription.isUnlock;
        public ItemType itemType { get; }
        public ItemDescription itemDescription { get; }

        public Item(Option option, int index)
        {
            text = option.text;
            price = option.price;
            points = option.points;
            effect = option.effect;
            itemKey = option.item;
            this.index = index;
            itemType = itemKey.StartsWith("dress") ? ItemType.Dress : ItemType.Hair;

            switch (itemType)
            {
                case ItemType.Dress:
                {
                    itemDescription = GameManager.Instance.scriptableBase.playerItemBase.GetDressByKey(itemKey);
                    break;
                }
                case ItemType.Hair:
                {
                    itemDescription = GameManager.Instance.scriptableBase.playerItemBase.GetHairByKey(itemKey);
                    break;
                }
            }
        }

        public void BuyItem()
        {
            itemDescription.BuyItem();
        }
    }


    public void Init(Option[] options, DressingRoomWindow dressingRoomWindow)
    {
        _dressingRoomWindow = dressingRoomWindow;
        for (var i = 0; i < options.Length; i++)
        {
            CreateItem(options[i], i);
        }
        var selectedStory = GameManager.Instance.selectedStory;
        var paidItems = GameManager.Instance.userData.progress.Find(value => value.storyId == selectedStory.id).paidItems;
        foreach(var item in _items)
        {
            if(paidItems.Contains(item.itemKey))
            {
                _boughtItems.Add(item);
            }
        }
        _dressingRoomWindow.OnItemChange += UpdateButtonsView;
    }

    private void OnDestroy()
    {
        _dressingRoomWindow.OnItemChange -= UpdateButtonsView;
    }

    public void CreateItem(Option option, int index)
    {
        var item = new Item(option, index);
        _items.Add(item);
        ItemDescriptions.Add(item.itemDescription);
    }

    public void UpdateButtonsView()
    {
        var isItemAlreadyPurchased = CurrentItemDescription.IsItemAlreadyPurchased();
        switch (isItemAlreadyPurchased)
        {
            case false:
                buyItemButton.gameObject.SetActive(true);
                selectItemButton.gameObject.SetActive(false);
                break;
            case true:
                buyItemButton.gameObject.SetActive(false);
                freeItemButton.gameObject.SetActive(false);
                //selectItemButton.gameObject.SetActive(true); 
                if(_selectedItemIndex==CurrentItem.index) //тут было красивое сокрытие кнопок. Либо убрать сокрытие, либо добавить немного логи
                    selectItemButton.gameObject.SetActive(false);// из-за сброса акта пришлось сделать нулевую кнопку всегда активной, ибо 
                else                                               // пати дресс первый КУПЛЕННЫЙ не имел кнопки выхода тогда                 
                     selectItemButton.gameObject.SetActive(true);               
                return;
        }
        

        if (CurrentItem.price > 0)
        {
            paidItemPriceText.SetText(CurrentItem.price.ToString());
            freeItemButton.gameObject.SetActive(false);
        }
        else
        {
            buyItemButton.gameObject.SetActive(false);
            freeItemButton.gameObject.SetActive(true);
        }

        /*descriptionText.SetText(_currentItem.text);
        priceText.SetText(_currentItem.price.ToString());*/
    }

    public void BuyItem()
    {
        ref var user = ref GameManager.Instance.userData;
        if (CurrentItem.price <= user.currencies.cash)//playerInformation.cash 
        {
            _boughtItems.Add(CurrentItem);
            user.currencies.cash -= CurrentItem.price;
            //playerInformation.cash -= CurrentItem.price;
            CurrentItem.BuyItem();
            SelectCurrentItem();
            UpdateButtonsView();
            //AskQuestion(); 
        }
        else
        {
            WindowsManager.Instance.CreateWindow<NoMoneyPopupWindow>();
        }
    }

    public void SelectCurrentItem()
    {      
        SelectItem(CurrentItem);
    }

    private void SelectItem(Item itemToSelect)
    {
        if(!_boughtItems.Contains(CurrentItem))
        {
            _boughtItems.Add(CurrentItem);
        }  
        var user = GameManager.Instance.userData;
        var selectedStory = GameManager.Instance.selectedStory;
        var progress = user.progress.Find(value => value.storyId == selectedStory.id);

        switch (CurrentItem.itemType)
        {
            case ItemType.Dress:
                Debug.Log($"_variableToSet : {_dressingRoomWindow.VariableToSet}");
                if (_dressingRoomWindow.VariableToSet == "partydress")
                {
                    progress.partydress = itemToSelect.itemKey;
                    Debug.Log("zapisivayem: "+ itemToSelect.itemKey);
                }
                else
                {
                    progress.heroDressKey = itemToSelect.itemKey;
                }
                break;
            case ItemType.Hair:
                progress.heroHairKey = itemToSelect.itemKey;
                break;
        }
        _selectedItemIndex=CurrentItem.index;
        if(!_isPlayerStopShopping)
        {
            UpdateButtonsView();
            selectItemButton.gameObject.SetActive(false);
            ContinueShopping();
            //AskQuestion();   
        }
           

    }

    private void AskQuestion()
    {
        if (!_isPlayerContinueShopping)
        {
            var shopQuestionWindow = WindowsManager.Instance.CreateWindow<ShopQuestionWindow>();
            shopQuestionWindow.OnExitButtonClick += ContinueGame;
            shopQuestionWindow.OnContinueShoppingButtonClock += ContinueShopping;
        }
    }

    public void SelectFreeItem()
    {
        BuyItem();
        SelectCurrentItem();
    }
    public void ContinueGame()
    {
        var bestItem = GetBestBuyItem();
        SelectItem(CurrentItem);
        OnShopComplete?.Invoke(CurrentItem.index);        
        OnShopCompleteBestItem?.Invoke(CurrentItem.index, bestItem.points);
    }

    public void ContinueGameWithBestItem()
    {
        _isPlayerStopShopping = true;    
        var bestItem = GetBestBuyItem();
        SelectItem(bestItem);
        OnShopComplete?.Invoke(bestItem.index);        
    }

    public void ContinueShopping()
    {
        _isPlayerContinueShopping = true;
        continueGameButton.gameObject.SetActive(true);
    }

    public void ContinueGameWithSelectedItem()
    {
        var bestItem = GetBestBuyItem();
        OnShopComplete?.Invoke(_selectedItemIndex);
        OnShopCompleteBestItem?.Invoke(CurrentItem.index, bestItem.points);      
    }
    private Item GetBestBuyItem()
    {
        int bestPrice = _boughtItems.Max(i => i.price);
        return _boughtItems.Find(value => value.price == bestPrice);
    }
}