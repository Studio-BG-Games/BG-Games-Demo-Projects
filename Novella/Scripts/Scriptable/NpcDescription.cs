using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NpcDescription", menuName = "ScriptableObjects/NpcDescription", order = 1)]
public class NpcDescription : ScriptableObject
{
    public bool npcWithHair
    {
        get
        {
            var hair = dresses.Find(value => value.name.StartsWith("hair"));
            return hair != null;
        }
    }
    
    [SerializeField] private List<Sprite> emotions = new List<Sprite>();
    [SerializeField] private List<Sprite> dresses = new List<Sprite>();
    [SerializeField] private Sprite hair;
    [SerializeField] private Sprite mask;
    [SerializeField] private Sprite body;
    
    public float scale;
    public float offsetX;
    public float offsetY;

    public Sprite GetEmotionByKey(string key)
    {
        return emotions.Find(value => value.name == key);
    }

    public Sprite GetDressByKey(string key)
    {
        return dresses.Count == 0 ? null : dresses.Find(value => value.name == key);
    }

    public Sprite GetBodySprite()
    {
        return body;
    }

    public Sprite GetHairSprite()
    {
        return hair;
    }

    public Sprite GetMaskSprite()
    {
        return mask;
    }
}