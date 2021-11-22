using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CircleColor : MonoBehaviour
{
	public Image Image;
	public ConfigColorEditor configColor;

	private void Start()
	{
		configColor = new ConfigColorEditor(Image.color);
	}

	public void Click()
	{
		ToolsEditor.Instance.CurrentconfigColor = configColor;
	}


}
