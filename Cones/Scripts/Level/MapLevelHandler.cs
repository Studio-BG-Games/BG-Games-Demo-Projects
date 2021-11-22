using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapLevelHandler : MonoBehaviour
{
	public List<LevelButton> LeveButtons = new List<LevelButton>();
	public Color ColorNoActive;
	public Color ColorActive;
	public Color ColorComplet;

	public void UpdateLevelButton()
	{
		Level tempLevel = default;
		foreach (var level in GameHandler.Instance.Levels)
		{
			if(level.isComplet == false)
			{
				tempLevel = level;
				break;
			}
		}
		foreach (var level in GameHandler.Instance.Levels)
		{
			if (level.isComplet)
			{
				LeveButtons[level.Id - 1].SetColor(ColorComplet);
				LeveButtons[level.Id - 1].GetComponent<Button>().interactable = true;
			}
			else
			{
				LeveButtons[level.Id - 1].SetColor(ColorNoActive);
				LeveButtons[level.Id - 1].GetComponent<Button>().interactable = false;
			}
		}
		for (int i = GameHandler.Instance.Levels.Count; i < LeveButtons.Count; i++)
		{
			LeveButtons[i].SetColor(ColorNoActive);
			LeveButtons[i].GetComponent<Button>().interactable = false;
		}
		foreach (var levelButton in LeveButtons)
		{
			if (tempLevel != null && levelButton.LevelId == tempLevel.Id)
			{
				levelButton.SetColor(ColorActive);
				levelButton.GetComponent<Button>().interactable = true;
			}
		}
	}
}
