using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddButton : MonoBehaviour
{
	[SerializeField] private RectTransform _rectTransform;
	[SerializeField] private Text Text;

	public bool isAdd = true;

	public void Add()
	{
		if (isAdd)
		{
			EditorHandler.Instance.AddCones();
			_rectTransform.SetAsLastSibling();
		}
		else
		{
			EditorHandler.Instance.DelCones();
			_rectTransform.SetAsLastSibling();
		}
		
	}

	public void Reversive()
	{
		isAdd = !isAdd;
		if (isAdd)
		{
			Text.text = "+";
		}
		else
		{
			Text.text = "-";
		}
	}
}
