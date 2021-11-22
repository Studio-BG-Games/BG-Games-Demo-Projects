using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Abstract
{
	public class SetColorButton : MonoBehaviour
	{
		[SerializeField] private ColorType _type;
		public void SetColor()
		{
			ColorHanlder.Instance.SetColor(_type);
		}
	}
}
