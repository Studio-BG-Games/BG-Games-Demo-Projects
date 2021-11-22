using UnityEngine;

[CreateAssetMenu(fileName = "picture", menuName = "CoreGame/Pictures/picture", order = 2)]
public class Picture : ScriptableObject
{
	[SerializeField] private string _id;
	[SerializeField] private string _name;
	[SerializeField] private Category _category;
	[SerializeField] private Sprite _sprite;

	public string Id { get{ return _id; } }
	public string Name { get{ return _name; } }
	public Category Category{ get{ return _category; } }
	public Sprite Sprite { get{ return _sprite; } }

}
