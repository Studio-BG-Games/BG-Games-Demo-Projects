using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Red : IColorably
{
	public Color Color { get; set; }
	public Red()
	{
		Color = Color.red;
	}
	public void SetColor(object colorably)
	{
		switch (colorably)
		{
			case Image image:
				image.DOColor(Color, 1f);
				break;
			case SpriteRenderer spriteRenderer:
				spriteRenderer.DOColor(Color, 2f);
				break;
		}
	}
}
