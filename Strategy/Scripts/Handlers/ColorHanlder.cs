using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ColorHanlder : MonoBehaviour
{
	public static ColorHanlder Instance;
	[SerializeField] private ColorPanel _colorPanel;
	private IColorably[] Colors;


	private void Awake()
	{
		Instance = this;
		Colors = new IColorably[]
		{
			new Red(),
			new Blue(),
			new Green()
		};
	}
	public void SetColor(ColorType color)
	{
		Colors[(int)color].SetColor(_colorPanel.Sprite);
	}
}
