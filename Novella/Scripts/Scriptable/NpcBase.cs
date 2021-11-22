using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NpcBase", menuName = "ScriptableObjects/NpcBase", order = 1)]
public class NpcBase : ScriptableObject
{
   [SerializeField] private List<NpcDescription> npcs = new List<NpcDescription>();

   public NpcDescription FindNpcByKey(string key)
   {
      return npcs.Find(value => value.name == key);
   }
}
