using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Green : IColorably
{
	public Color Color { get; set; }
	public Green()
	{
		Color = Color.green;
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
