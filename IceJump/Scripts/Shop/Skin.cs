using UnityEngine;

[CreateAssetMenu(fileName = "Skin", menuName = "Shop/Products")]
public class Skin : ScriptableObject
{
	public string Name;
	public Sprite SpriteSkin;
	public int Price;
	public bool isDefault = false;

}
