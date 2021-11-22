using System;
using System.Collections.Generic;
using System.Diagnostics;
using Scripts.Managers;
using Scripts.Serializables.Story;
using Scripts.Serializables.User;
using Scripts.UISystem;
using DG.Tweening;
using GeneratedUI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;
using Scripts;

public class DressingRoomWindow : WindowController
{
    public event Action<int> OnShopComplete;
    public event Action<int, int> OnShopCompleteBestItem;
    public event Action OnItemChange;
    public event Action OnCharacterCustomizationComplete;

    [SerializeField] private List<ItemContainer> itemContainers = new List<ItemContainer>();
    [SerializeField] private ShopPanel shopPanel;
    [SerializeField] private GameObject doneButton;
    [SerializeField] private GameObject selectItemPanel;
    [SerializeField] private GameObject closeWindowButton;
    [SerializeField] private GameObject subMenuButtons;
    [SerializeField] private TextMeshProUGUI doneButtonTMP, continueButtonTMP, selectButtonTMP, freeButtonTMP, paidButtonTMP;
    private string doneButtonkey = "ready", 
    continueButtonKey = "closeDressingroom", 
    selectButtonKey = "puton", 
    freeButtonKey = "puton", 
    paidButtonKey = "puton";
    [SerializeField] private Image arrow;
    [SerializeField] private Sprite arrowUp, arrowDown;
    [SerializeField] private Animation movePanel;

    [SerializeField] private RectTransform characterRect;
    [SerializeField] private CanvasGroup characterCanvasGroup;
    [SerializeField] private TextMeshProUGUI descriptionText;

    [SerializeField] private float maxDownPos = -50f;
    [SerializeField] private float maxRightPos = 50f;
    [SerializeField] private float animSpeed = 0.3f;

    private bool isHide = false;
    public ItemContainer currentItemContainer { get; private set; }

    private string _variableToSet;
    public string VariableToSet => _variableToSet;
    
    private string _localizedItemName = "";
    private Story _selectedStory;

    public enum WindowType
    {
        Personalize,
        Wardrobe,
        Shop
    }

    /*
     * 0 = hairs
     * 1 = dress
     * 2 = body
     */

    [Serializable]
    public class ItemContainer
    {
        public ItemDescription currentItem;
        [SerializeField] private Image itemContainer;
        private int _index;
        private List<ItemDescription> _items = new List<ItemDescription>();

        public void NextItem(bool side)
        {
            var indexChanger = side ? 1 : -1;
            _index += indexChanger;
            if (_index > _items.Count - 1) _index = 0;
            if (_index < 0) _index = _items.Count - 1;
            currentItem = _items[_index];
        }

        public void AddItemsToList(List<ItemDescription> itemDescriptions)
        {
            _items.AddRange(itemDescriptions);
            currentItem = _items[_index];
            UpdateContainerView(currentItem.sprite);
        }

        public void SetItemsList(List<ItemDescription> itemDescriptions)
        {
            _items.Clear();
            _items.AddRange(itemDescriptions);
            currentItem = _items[_index];
            UpdateContainerView(currentItem.sprite);
        }

        public void UpdateContainerView(Sprite sprite)
        {
            itemContainer.sprite = sprite;
        }

        public void UpdateCurrentItemFromContainerView()
        {
            _index = _items.IndexOf(_items.Find(value => value.sprite == itemContainer.sprite));
            currentItem = _items[_index];
        }
    }

    private void Start()
    {
        
        shopPanel.OnShopCompleteBestItem += ShopCompleteBestItem;
        shopPanel.OnShopComplete += ShopComplete;
        
        var selectedStory = GameManager.Instance.selectedStory;        
        var doneButtonText = selectedStory.general.Find(value => value.system == doneButtonkey);
        var continueButtonText = selectedStory.general.Find(value => value.system == continueButtonKey);
        var selectButtonText = selectedStory.general.Find(value => value.system == selectButtonKey);
        var freeButtonText = selectedStory.general.Find(value => value.system == freeButtonKey);
        var paidButtonText = selectedStory.general.Find(value => value.system == paidButtonKey);
        switch(PlayerPrefs.GetString("language"))
        {
			case "rus":
                doneButtonTMP.text = doneButtonText.russian;
				continueButtonTMP.text = continueButtonText.russian;                
                selectButtonTMP.text = selectButtonText.russian;
                freeButtonTMP.text = freeButtonText.russian;
                paidButtonTMP.text = paidButtonText.russian;
                break;
			case "eng":
				doneButtonTMP.text = doneButtonText.english;
				continueButtonTMP.text = continueButtonText.english;                
                selectButtonTMP.text = selectButtonText.english;
                freeButtonTMP.text = freeButtonText.english;
                paidButtonTMP.text = paidButtonText.english;
				break;
		}            
    }

    private void OnDestroy()
    {
        
        shopPanel.OnShopCompleteBestItem -= ShopCompleteBestItem;
        shopPanel.OnShopComplete -= ShopComplete;
    }

    private void ShopComplete(int index)
    {
        OnShopComplete?.Invoke(index);
    }
    private void ShopCompleteBestItem(int i, int points)
    {
        
        OnShopCompleteBestItem?.Invoke(i, points);        
        CloseWindow();
    }

    public void ChooseWindowType(WindowType windowType)
    {
        switch (windowType)
        {
            case WindowType.Personalize:
                {
                    LoadPersonalizeSettings();
                    break;
                }
            case WindowType.Wardrobe:
                {
                    LoadWardrobeSettings();
                    break;
                }
            case WindowType.Shop:
                {
                    LoadShopSettings();
                    break;
                }
        }
    }

    public void OpenShop(Option[] options, string variable = null)
    {
        _variableToSet = variable;

        shopPanel.Init(options, this);
        LoadShopSettings();

        var items = shopPanel.ItemDescriptions;
        LoadShopItems(shopPanel.ShopItemType, items);

        shopPanel.UpdateButtonsView();
    }

    private void LoadShopSettings()
    {
        subMenuButtons.SetActive(true);
        shopPanel.gameObject.SetActive(true);
        selectItemPanel.SetActive(false);
        closeWindowButton.SetActive(false);
        doneButton.SetActive(false);
    }

    private void LoadPersonalizeSettings()
    {
        shopPanel.gameObject.SetActive(false);
        closeWindowButton.SetActive(false);
        doneButton.SetActive(true);
        LoadPersonalizeItems();
        _selectedStory = GameManager.Instance.selectedStory;        
        var itemGeneral = _selectedStory.general.Find(value => value.system == currentItemContainer.currentItem.itemKey);
        switch (PlayerPrefs.GetString("language"))
        {
            case "rus": 
                _localizedItemName = itemGeneral.russian;
                break;
            case "eng":
                _localizedItemName = itemGeneral.english;
                break;
        }
        UpdateDescriptionText(_localizedItemName);
        
    }

    private void LoadWardrobeSettings()
    {
        selectItemPanel.SetActive(true);
        shopPanel.gameObject.SetActive(false);
        closeWindowButton.SetActive(true);
        doneButton.SetActive(true);
        LoadWardrobeItems();
        _selectedStory = GameManager.Instance.selectedStory; 
        var itemGeneral = _selectedStory.general.Find(value => value.system == currentItemContainer.currentItem.itemKey);
        switch (PlayerPrefs.GetString("language"))
        {
            case "rus": 
                _localizedItemName = itemGeneral.russian;
                break;
            case "eng":
                _localizedItemName = itemGeneral.english;
                break;
        }
        UpdateDescriptionText(_localizedItemName);
    }

    public void NextItem(bool side)
    {
        currentItemContainer.NextItem(side);
        PlayAnimation(() => currentItemContainer.UpdateContainerView(currentItemContainer.currentItem.sprite));
        _selectedStory = GameManager.Instance.selectedStory; 
        var itemGeneral = _selectedStory.general.Find(value => value.system == currentItemContainer.currentItem.itemKey);
        switch (PlayerPrefs.GetString("language"))
        {
            case "rus": 
                _localizedItemName = itemGeneral.russian;
                break;
            case "eng":
                _localizedItemName = itemGeneral.english;
                break;
        }
        UpdateDescriptionText(_localizedItemName);
        OnItemChange?.Invoke();
    }

    public void SelectItemContainer(int indexContainer)
    {
        currentItemContainer = itemContainers[indexContainer];
        _selectedStory = GameManager.Instance.selectedStory; 
        var itemGeneral = _selectedStory.general.Find(value => value.system == currentItemContainer.currentItem.itemKey);
        switch (PlayerPrefs.GetString("language"))
        {
            case "rus": 
                _localizedItemName = itemGeneral.russian;
                break;
            case "eng":
                _localizedItemName = itemGeneral.english;
                break;
        }
        UpdateDescriptionText(_localizedItemName);
    }

    private void UpdateDescriptionText(string text)
    {        
        descriptionText.SetText(text);
    }

    private void PlayAnimation(Action middle = null)
    {
        var startPos = characterRect.rect.y;
        startPos = startPos * -1;
        var sequence = DOTween.Sequence();
        sequence.Insert(0, characterRect.DOAnchorPosX(maxRightPos, animSpeed));
        sequence.Insert(0, characterRect.DOAnchorPosY(startPos + maxDownPos, animSpeed));
        sequence.Insert(0, characterCanvasGroup.DOFade(0, animSpeed).OnComplete(() => middle?.Invoke()));
        sequence.Insert(animSpeed, characterRect.DOAnchorPosX(-maxRightPos, 0));
        sequence.Insert(animSpeed, characterRect.DOAnchorPosX(0f, animSpeed));
        sequence.Insert(animSpeed, characterRect.DOAnchorPosY(startPos, animSpeed));
        sequence.Insert(animSpeed, characterCanvasGroup.DOFade(1, animSpeed));
    }

    private void LoadShopItems(ShopPanel.ItemType shopType, List<ItemDescription> itemDescriptions)
    {
        var user = GameManager.Instance.userData;
        var selectedStory = GameManager.Instance.selectedStory;
        var progress = user.progress.Find(value => value.storyId == selectedStory.id);

        switch (shopType)
        {
            case ShopPanel.ItemType.Hair:
                itemContainers[0].SetItemsList(itemDescriptions);
                itemContainers[1]
                    .UpdateContainerView(progress.HeroDress.sprite);
                break;
            case ShopPanel.ItemType.Dress:
                itemContainers[1].SetItemsList(itemDescriptions);
                itemContainers[0]
                    .UpdateContainerView(progress.HeroHair.sprite);
                break;
        }

        itemContainers[2]
            .UpdateContainerView(GameManager.Instance.scriptableBase.playerItemBase.GetBodyByKey(progress.heroBodyKey)
                .body);
        /*.emotions.Find(value => value.name == normal);*/
        currentItemContainer = itemContainers[(int)shopType];
        _selectedStory = GameManager.Instance.selectedStory; 
        var itemGeneral = _selectedStory.general.Find(value => value.system == currentItemContainer.currentItem.itemKey);
        switch (PlayerPrefs.GetString("language"))
        {
            case "rus": 
                _localizedItemName = itemGeneral.russian;
                break;
            case "eng":
                _localizedItemName = itemGeneral.english;
                break;
        }
        UpdateDescriptionText(_localizedItemName);
    }

    private void LoadPersonalizeItems()
    {
        LoadDefaultItems();
        currentItemContainer = itemContainers[0];
    }

    private void LoadWardrobeItems()
    {
        LoadInventory();
        currentItemContainer = itemContainers[0];
        SetViewToCurrentItems();
    }

    private void LoadInventory()
    {
        var paidItems = GameManager.Instance.playerInformation.playerItemBase.GetPlayerInventoryItems();
        var hairItems = paidItems.FindAll(value => value.itemKey.StartsWith("hair"));
        var dressItems = paidItems.FindAll(value => value.itemKey.StartsWith("dress"));
        var bodyItems = paidItems.FindAll(value => value.itemKey.StartsWith("body"));

        itemContainers[0].AddItemsToList(hairItems);
        itemContainers[1].AddItemsToList(dressItems);
        itemContainers[2].AddItemsToList(bodyItems);
    }

    private void LoadDefaultItems()
    {
        itemContainers[0].AddItemsToList(GameManager.Instance.playerInformation.playerItemBase.GetDefaultHairs());
        itemContainers[1].AddItemsToList(GameManager.Instance.playerInformation.playerItemBase.GetDefaultDresses());
        itemContainers[2].AddItemsToList(GameManager.Instance.scriptableBase.playerItemBase.GetDefaultBodys());
    }

    private void SetViewToCurrentItems()
    {
        var user = GameManager.Instance.userData;
        var selectedStory = GameManager.Instance.selectedStory;
        var progress = user.progress.Find(value => value.storyId == selectedStory.id);

        itemContainers[0]
            .UpdateContainerView(progress.HeroHair.sprite);
        itemContainers[1]
            .UpdateContainerView(GameManager.Instance.playerInformation.playerItemBase
                .GetDressByKey(progress.heroDressKey).sprite);
        itemContainers[2]
            .UpdateContainerView(GameManager.Instance.playerInformation.playerItemBase
                .GetBodyByKey(progress.heroBodyKey).body);
        foreach (var itemContainer in itemContainers)
            itemContainer.UpdateCurrentItemFromContainerView();
    }

    public void SaveSetup()
    {
        var _story = GameManager.Instance.selectedStory;
        ref var _user = ref GameManager.Instance.userData;
        var _selectedStory = GameManager.Instance.selectedStory;
        var _storyProgress = _user.progress.Find(value => value.storyId == _selectedStory.id);
        var _act = _story.acts.Find(a => a.id == _storyProgress.actId);
        var _currentRecordIndex = _storyProgress.recordId;
        var _currentRecord = _act.records[_currentRecordIndex];

        _storyProgress.heroHairKey = itemContainers[0].currentItem.itemKey;
        _storyProgress.heroDressKey = itemContainers[1].currentItem.itemKey;
        _storyProgress.heroBodyKey = itemContainers[2].currentItem.itemKey;
        
        FirebaseManager.Instance.UpdateUserData();// тут валюта вроде не тратиться, но от бага подальше..
        //ES3.Save("user", _user);
        

        if ( _currentRecord.npc == "protagonist")
            PlayerPrefs.SetString("CloseWardrobe", "true");
        

        /*_playerSetup = new PlayerSetup();
        _playerSetup.SaveHairStyle(itemContainers[0].currentItem);
        _playerSetup.SaveDress(itemContainers[1].currentItem);
        _playerSetup.SaveBodyType(itemContainers[2].currentItem as BodyDescription);
        GameManager.Instance.playerInformation.SetPlayerSetup(_playerSetup);*/
        OnCharacterCustomizationComplete?.Invoke();
        CloseWindow();
    }

    public void MovePanel()
    {
        if(!isHide)
        {
            isHide = true;
            movePanel.Play("HideShop");
            arrow.sprite = arrowUp;
        }
        else
        {
            isHide = false;
            movePanel.Play("OpenShop");
            arrow.sprite = arrowDown;
        }

    }
}