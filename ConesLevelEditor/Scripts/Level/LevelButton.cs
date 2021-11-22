using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
	public int LevelId;
	public bool IsCurrentLevel;


	public void SetLevel()
	{
		AudioHandler.Instance.PlaySound(TypeSound.buttonInteract);
		GameHandler.Instance.StartLevel(GameHandler.Instance.Levels[LevelId - 1]);
		PausedHandler.Instance.CloseMapLevels();
		PausedHandler.Instance.CloseHomePanel();
	}

	public void SetColor(Color color)
	{
		GetComponent<Image>().color = color;
	}
}
