using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableBase", menuName = "ScriptableObjects/ScriptableBase", order = 1)]
public class ScriptableBase : ScriptableObject
{
    public NpcBase npcBase;
    public BackgroundBase backgroundBase;
    public PlayerItemBase playerItemBase;
}
