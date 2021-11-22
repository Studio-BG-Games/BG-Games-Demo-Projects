using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BackgroundBase", menuName = "ScriptableObjects/BackgroundBase", order = 1)]
public class BackgroundBase : ScriptableObject
{
   [SerializeField] private List<BackgroundDescription> backgrounds = new List<BackgroundDescription>();
   public List<BackgroundDescription> Backgrounds => backgrounds;
   public BackgroundDescription GetBackgroundByKey(string key)
   {
      return backgrounds.Find(value => value.name == key);
   }
}
