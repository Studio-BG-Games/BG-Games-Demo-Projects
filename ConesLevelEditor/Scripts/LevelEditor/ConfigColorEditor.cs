using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigColorEditor
{
	public int Id;
	public Color Color;
	public ConfigColorEditor(Color color)
	{
		EditorHandler.Instance.ConfigColorEditor.Add(this);
		this.Color = color;
		this.Id = EditorHandler.Instance.ConfigColorEditor.Count;

	}
}
