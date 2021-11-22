using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Itibsoft.Utils.ToolsEngine;
using System;

public class ToolsEditor : MonoBehaviour
{
	public static ToolsEditor Instance;
	[SerializeField] private GameObject PanelActiveCircleTool;
	[SerializeField] private List<Tools> _tools = new List<Tools>();

	public Tools CurrentTools;
	private Tools OldTools;

	public ConfigColorEditor CurrentconfigColor;

	public void Awake()
	{
		Instance = this;
	}

	public void SetActivePanel()
	{
		bool isCheckTool = false;
		if(OldTools == null || OldTools != CurrentTools)
		{
			CurrentTools.SetColorOutline(Color.red);
			isCheckTool = false;
		}
		else
		{
			CurrentTools.SetColorOutline(Color.black);
			CurrentTools = null;
			isCheckTool = true;
		}
		OldTools = CurrentTools;

		if (ToolsEngine.IsNull(CurrentTools) && CurrentTools.TypeTool == ToolsType.circles && !isCheckTool)
		{
			PanelActiveCircleTool.SetActive(true);
		}
		else
		{
			PanelActiveCircleTool.SetActive(false);
		}
		

		foreach (var tool in _tools)
		{
			if (tool == CurrentTools) continue;

			tool.SetColorOutline(Color.black);
		}
	}
}
