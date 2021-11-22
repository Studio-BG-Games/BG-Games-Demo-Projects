using System.Collections;
using System.Collections.Generic;
using Scripts.Managers;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDescription", menuName = "ScriptableObjects/ItemDescription", order = 1)]
public class ItemDescription : ScriptableObject
{
    public Sprite sprite;
    public string itemKey;
    public bool isDefaultItem;
    
    public void BuyItem()
    {
        var user = GameManager.Instance.userData;
        var selectedStory = GameManager.Instance.selectedStory;
        var progress = user.progress.Find(value => value.storyId == selectedStory.id);
        progress.paidItems.Add(itemKey);
    }

    public bool IsItemAlreadyPurchased()
    {
        var user = GameManager.Instance.userData;
        var selectedStory = GameManager.Instance.selectedStory;
        var progress = user.progress.Find(value => value.storyId == selectedStory.id);
        var item = progress.paidItems.Find(value => value.Contains(itemKey));
        return item != null;
    }
}
