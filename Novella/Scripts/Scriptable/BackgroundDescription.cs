using Scripts.Managers;
using UnityEngine;

[CreateAssetMenu(fileName = "BackgroundDescription", menuName = "ScriptableObjects/BackgroundDescription", order = 1)]
public class BackgroundDescription : ScriptableObject
{
    [SerializeField] private Sprite sprite;
    [SerializeField] private int animationKey;
    [SerializeField] private float backgroundOffsetPercent;
    public Sprite Sprite => sprite;
    public string Key => sprite.name;
    public int AnimationKey => animationKey;
    public float BackgroundOffsetPercent => backgroundOffsetPercent;

    public void BuyItem()
    {
        var user = GameManager.Instance.userData;
        var selectedStory = GameManager.Instance.selectedStory;
        var progress = user.progress.Find(value => value.storyId == selectedStory.id);
        progress.paidItems.Add(Key);
    }

    public bool IsItemAlreadyPurchased()
    {
        var user = GameManager.Instance.userData;
        var selectedStory = GameManager.Instance.selectedStory;
        var progress = user.progress.Find(value => value.storyId == selectedStory.id);
        var item = progress.paidItems.Find(value => value.Contains(Key));
        return item != null;
    }

}
