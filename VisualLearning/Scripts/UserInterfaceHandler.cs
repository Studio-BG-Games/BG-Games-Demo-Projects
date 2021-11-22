using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInterfaceHandler : MonoBehaviour
{
	[SerializeField] private SwitchButton[] _switchButton;
	[SerializeField] private Text _nameText;

	public Text NameText{ get{ return _nameText; } }
	public SwitchButton[] SwitchButton{ get{ return _switchButton; } }
}
