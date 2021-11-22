using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameHandler : MonoBehaviour
{
	public static GameHandler Instance;
	public List<Cone> SelectedCones = new List<Cone>();
	public List<Level> Levels = new List<Level>();
	public Level CurrentLevel;
	[SerializeField] private Text _levelText;
	[SerializeField] private MapLevelHandler _mapLevelHandler;
	public void SelectedCone(Cone cone)
	{
		SelectedCones.Add(cone);
	}

	private void Awake()
	{
		Instance = this;
		foreach (var level in Levels)
		{
			if(PlayerPrefs.HasKey("level_" + (level.Id - 1)))
			{
				level.isComplet = true;
				continue;
			}
			else
			{
				level.isComplet = false;
				StartLevel(level);
				return;
			}
		}
		StartLevel(Levels[0]);
	}
	public void StartLevel(Level levelStart)
	{

		_levelText.text = $"������� {levelStart.Id}";
		if (CurrentLevel != null)
		{
			BreakLevel();
		}
		CurrentLevel = levelStart;
		_mapLevelHandler.UpdateLevelButton();
		foreach (var level in Levels)
		{

			if (level != levelStart) 
			{
				level.gameObject.SetActive(false);
			}
			levelStart.gameObject.SetActive(true);
		}
	}

	public void CheckLevel()
	{
		var temp = 0;
		foreach (var cone in CurrentLevel.Cones)
		{
			if (cone.isComplet)
			{
				temp++;
			}

		}
		if(temp == CurrentLevel.CountCones)
		{
			AudioHandler.Instance.PlaySound(TypeSound.win);

			if (CurrentLevel.isComplet)
			{
				CurrentLevel.RewardMolecules = 0;
			}
			PausedHandler.Instance.OpenWinPanel();
			MoleculesBank.Instance.AddMolecules(CurrentLevel.RewardMolecules);
			CurrentLevel.isComplet = true;
			PlayerPrefs.SetInt("level_" + (CurrentLevel.Id - 1), 1);

		}
			
	}
	public void NextLevel()
	{
		if (CurrentLevel.Id > Levels.Count - 1)
		{
			CurrentLevel.gameObject.SetActive(false);
			_mapLevelHandler.UpdateLevelButton();
			PausedHandler.Instance.CloseWinPanel();
			PausedHandler.Instance.OpenMapLevels(true);
			AudioHandler.Instance.PlaySound(TypeSound.popup);
			PausedHandler.Instance.ShowPopupHelper("������ �����������! ���������� ��������� ����������...");
			return;
		}
		StartLevel(Levels[CurrentLevel.Id]);
		PausedHandler.Instance.CloseWinPanel();
	}

	public void BreakLevel()
	{
		SelectedCones = new List<Cone>();
		foreach (var Cone in CurrentLevel.Cones)
		{
			Cone.isComplet = false;
			if (Cone.CircleOut)
			{
				Cone.CircleOut.isOut = false;
				Cone.CircleOut = null;
			}
			foreach (var cell in Cone.Cells)
			{
				cell.Restart();
			}
		}
	}
}
