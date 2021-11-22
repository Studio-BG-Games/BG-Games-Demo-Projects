using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BodyDescription", menuName = "ScriptableObjects/BodyDescription", order = 1)]
public class BodyDescription : ItemDescription
{
   // public Sprite previewSprite;
    public Sprite body;
    public List<Sprite> emotions = new List<Sprite>();
}
