using System.Collections.Generic;
using Scripts.Managers;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerCustomizationBase", menuName = "ScriptableObjects/PlayerCustomizationBase",
    order = 1)]
public class PlayerItemBase : ScriptableObject
{
    [SerializeField] private List<ItemDescription> hairs = new List<ItemDescription>();
    [SerializeField] private List<ItemDescription> dresses = new List<ItemDescription>();
    [SerializeField] private List<ItemDescription> bodyTypes = new List<ItemDescription>();

    public ItemDescription GetHairByKey(string hairKey)
    {
        var hairItem = hairs.Find(value => value.itemKey.Equals(hairKey));
        return hairItem;
    }

    public ItemDescription GetDressByKey(string dressKey)
    {
        var dressItem = dresses.Find(value => value.itemKey.Equals(dressKey));
        return dressItem;
    }

    public BodyDescription GetBodyByKey(string key)
    {
        var bodyItem = bodyTypes.Find(value => value.itemKey.Equals(key));
        return bodyItem as BodyDescription;
    }

    public List<ItemDescription> GetDefaultBodys()
    {
        List<ItemDescription> defaultBodies = new List<ItemDescription>();
        defaultBodies.AddRange(bodyTypes.FindAll(value => value.isDefaultItem));
        return defaultBodies;
    }

    public List<ItemDescription> GetDefaultHairs()
    {
        List<ItemDescription> defaultHair = new List<ItemDescription>();
        defaultHair.AddRange(hairs.FindAll(value => value.isDefaultItem));
        return defaultHair;
    }

    public List<ItemDescription> GetDefaultDresses()
    {
        List<ItemDescription> defaultDresses = new List<ItemDescription>();
        defaultDresses.AddRange(dresses.FindAll(value => value.isDefaultItem));
        return defaultDresses;
    }

    public List<ItemDescription> GetPaidItems()
    {
        var selectedStory = GameManager.Instance.selectedStory;
        var progress = GameManager.Instance.userData.progress.Find(value => value.storyId == selectedStory.id);
        var itemsKeys = progress.paidItems;
        List<ItemDescription> paidItems = new List<ItemDescription>();
        paidItems.AddRange(hairs.FindAll(value => value.itemKey == itemsKeys.Find(value2 => value2 == value.itemKey)));
        paidItems.AddRange(dresses.FindAll(value =>
            value.itemKey == itemsKeys.Find(value2 => value2 == value.itemKey)));
        paidItems.AddRange(bodyTypes);
        return paidItems;
    }

    public List<ItemDescription> GetPlayerInventoryItems()
    {
        var inventoryItems = GetPaidItems();
        inventoryItems.AddRange(GetDefaultDresses());
        inventoryItems.AddRange(GetDefaultHairs());
        return inventoryItems;
    }
}