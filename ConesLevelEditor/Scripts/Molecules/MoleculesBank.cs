using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoleculesBank : MonoBehaviour
{
	public static MoleculesBank Instance;
	public static Action<int> OnChangedMoleculesAction;
	private int _countMolecules;

	private void Start()
	{
		Instance = this;
		if (!PlayerPrefs.HasKey("molecules"))
		{
			_countMolecules = default;
		}
		else
		{
			_countMolecules = PlayerPrefs.GetInt("molecules");
		}
		OnChangedMoleculesAction?.Invoke(_countMolecules);
	}

	public void AddMolecules(int count)
	{
		_countMolecules += count;
		OnChangedMoleculesAction?.Invoke(_countMolecules);
		PlayerPrefs.SetInt("molecules", _countMolecules);
	}

	public void PutMolecules(int count)
	{
		_countMolecules -= count;
		OnChangedMoleculesAction?.Invoke(_countMolecules);
		PlayerPrefs.SetInt("molecules", _countMolecules);
	}
}
