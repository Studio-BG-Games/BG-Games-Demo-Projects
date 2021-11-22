using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum ToolsType
{
	circles,
	zalivka,
	lastik
	
}
public class Tools : MonoBehaviour
{

	public ToolsType TypeTool;
	[SerializeField] private Outline _outline;
	public void SetTools()
	{
		ToolsEditor.Instance.CurrentTools = this;
		ToolsEditor.Instance.SetActivePanel();
	}

	public void SetColorOutline(Color color)
	{
		_outline.effectColor = color;
	}
}
