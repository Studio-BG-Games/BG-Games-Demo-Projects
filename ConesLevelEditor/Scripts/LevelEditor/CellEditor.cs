using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellEditor : MonoBehaviour
{

	public int Id;
	private Transform _transform;
	public Transform Transform { get { return _transform; } }
	public CircleEditor CircleBusy;
	public CircleEditor StartCircleBusy;
	public bool IsBusy;


	public bool SavedIsBusy;
	public CircleEditor SavedCirclyBusy;
	public void SaveCurrent()
	{
		if(CircleBusy != null)
		{
			SavedCirclyBusy = CircleBusy;
		}
		else
		{
			SavedCirclyBusy = null;
		}
		SavedIsBusy = IsBusy;
	}

	public void UndoToSave()
	{
		IsBusy = SavedIsBusy;
		if(SavedCirclyBusy != null)
		{
			CircleBusy = SavedCirclyBusy;
		}
		else
		{
			CircleBusy = null;
			StartCircleBusy.Image.color = new Color(0, 0, 0, 0f);
			StartCircleBusy.CurrentColor = new Color(0, 0, 0, 0f);
		}
		IsBusy = SavedIsBusy;
	}

	private void Start()
	{
		_transform = GetComponent<Transform>();
		if (CircleBusy != null)
		{
			CircleBusy.OnActiveToCellAction += DellCircle;
			CircleBusy.OnAddCircleAction += AddCircle;
			StartCircleBusy = CircleBusy;
			CircleBusy.transform.position = transform.position;
			CircleBusy.SetStartPosition();
		}
	}
	public void DellCircle()
	{
		CircleBusy = null;
		IsBusy = false;
	}
	public void AddCircle(CircleEditor circle)
	{
		CircleBusy = circle;
		IsBusy = true;
	}
	public void Restart()
	{
		if (StartCircleBusy != null)
		{
			CircleBusy = StartCircleBusy;
			CircleBusy.transform.localPosition = CircleBusy.StartPosition;
			CircleBusy.Id = default;
			IsBusy = true;
		}
		else
		{
			CircleBusy = null;
			IsBusy = false;

		}
	}
}
