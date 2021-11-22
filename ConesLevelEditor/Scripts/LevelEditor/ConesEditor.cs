using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConesEditor : MonoBehaviour
{
	public List<CellEditor> Cells = new List<CellEditor>();
	public bool isReady = false;
	public Transform OutPositionToCone;
	public CircleEditor CircleOut;
	public bool isComplet;

	public GameObject Button;

	public void OnTap()
	{
		EditorHandler.Instance.SelectedCone(this);
		var check = false;
		foreach (var cellCheck in Cells)
		{
			if (cellCheck.CircleBusy != null || EditorHandler.Instance.SelectedCones.Count == 2)
			{
				if (!CheckFill())
				{
					check = true;
				}
				else
				{
					EditorHandler.Instance.SelectedCones[0].Cells.Reverse();
					foreach (var cell in EditorHandler.Instance.SelectedCones[0].Cells)
					{
						if (!cell.IsBusy && EditorHandler.Instance.SelectedCones[0].CircleOut)
						{
							cell.CircleBusy = EditorHandler.Instance.SelectedCones[0].CircleOut;
							cell.CircleBusy.isOut = false;
							cell.CircleBusy.SetPosition(cell.Transform.position);
							cell.IsBusy = true;
							EditorHandler.Instance.SelectedCones[0].CircleOut = null;

							break;
						}
					}
					EditorHandler.Instance.SelectedCones[0].Cells.Reverse();
				}
				break;
			}
			else
			{
				check = false;
			}
		}
		if (!check)
		{
			AudioHandler.Instance.PlaySound(TypeSound.failMove);
			EditorHandler.Instance.SelectedCones = new List<ConesEditor>();
			return;
		}




		if (EditorHandler.Instance.SelectedCones.Count < 2)
		{
			InAndOutCircleFromCell();
		}
		else if (EditorHandler.Instance.SelectedCones[0] == EditorHandler.Instance.SelectedCones[1])
		{
			InAndOutCircleFromCell();
			CheckCellsFill();
			EditorHandler.Instance.SelectedCones = new List<ConesEditor>();
			return;

		}
		else if (!CheckCircle())
		{
			AudioHandler.Instance.PlaySound(TypeSound.failMove);
			EditorHandler.Instance.SelectedCones[0].Cells.Reverse();
			foreach (var cell in EditorHandler.Instance.SelectedCones[0].Cells)
			{
				if (!cell.IsBusy && EditorHandler.Instance.SelectedCones[0].CircleOut)
				{
					cell.CircleBusy = EditorHandler.Instance.SelectedCones[0].CircleOut;
					cell.CircleBusy.isOut = false;
					cell.CircleBusy.SetPosition(cell.Transform.position);
					cell.IsBusy = true;
					EditorHandler.Instance.SelectedCones[0].CircleOut = null;
					break;
				}
			}
			EditorHandler.Instance.SelectedCones[0].Cells.Reverse();
			EditorHandler.Instance.SelectedCones = new List<ConesEditor>();
			return;
		}
		else
		{
			AudioHandler.Instance.PlaySound(TypeSound.moveTo);
			EditorHandler.Instance.SelectedCones[1].Cells.Reverse();
			foreach (var cell in EditorHandler.Instance.SelectedCones[1].Cells)
			{
				if (cell.IsBusy == false)
				{
					cell.IsBusy = true;
					cell.CircleBusy = EditorHandler.Instance.SelectedCones[0].CircleOut;
					EditorHandler.Instance.SelectedCones[0].CircleOut.SetPosition(cell.Transform.position);
					EditorHandler.Instance.SelectedCones[0].CircleOut.isOut = false;
					EditorHandler.Instance.SelectedCones[0].CircleOut = null;

					break;

				}
			}
			EditorHandler.Instance.SelectedCones[1].Cells.Reverse();
			EditorHandler.Instance.SelectedCones = new List<ConesEditor>();

		}
		CheckCellsFill();

	}

	private bool CheckCircle()
	{
		var check = false;
		CircleEditor circleTemp = default;
		foreach (var cell in EditorHandler.Instance.SelectedCones[1].Cells)
		{
			if (cell.IsBusy)
			{
				circleTemp = cell.CircleBusy;
				break;
			}
		}
		if (circleTemp != null)
		{
			if (circleTemp.Id == EditorHandler.Instance.SelectedCones[0].CircleOut.Id)
			{
				check = true;
			}
		}
		else
		{
			if (!EditorHandler.Instance.SelectedCones[1].Cells[Cells.Count - 1].IsBusy)
			{
				check = true;
			}
			else
			{
				check = false;
			}

		}

		return check;
	}
	private bool CheckFill()
	{
		bool check = true;
		if (EditorHandler.Instance.SelectedCones.Count != 2)
		{
			return false;
		}
		foreach (var cellCheck in EditorHandler.Instance.SelectedCones[1].Cells)
		{
			if (!cellCheck.IsBusy)
			{
				check = false;
			}
		}
		return check;
	}
	public void InAndOutCircleFromCell()
	{
		AudioHandler.Instance.PlaySound(TypeSound.move);
		foreach (var cell in Cells)
		{
			if (cell.IsBusy && CircleOut == null)
			{
				if (!cell.CircleBusy.isOut)
				{
					cell.CircleBusy.SetPosition(OutPositionToCone.position);
					cell.CircleBusy.isOut = true;
					cell.IsBusy = false;
					CircleOut = cell.CircleBusy;
					cell.CircleBusy = null;

				}
				return;
			}
		}
		Cells.Reverse();
		foreach (var cell in Cells)
		{
			if (!cell.IsBusy && CircleOut)
			{
				cell.CircleBusy = CircleOut;
				cell.CircleBusy.isOut = false;
				cell.CircleBusy.SetPosition(cell.Transform.position);
				cell.IsBusy = true;
				CircleOut = null;
				Cells.Reverse();
				return;
			}
		}
		Cells.Reverse();

	}

	public bool CheckCellsFill()
	{
		foreach (var cell in Cells)
		{
			if (cell.IsBusy)
			{
				continue;
			}
			else
			{
				isComplet = false;
				return false;
			}
		}
		if (Cells[0].CircleBusy.Id == Cells[Cells.Count - 1].CircleBusy.Id)
		{

			isComplet = true;
			//EditorHandler.Instance.CheckLevel();
			return true;
		}
		else
		{
			isComplet = false;
			return false;
		}
	}
}
