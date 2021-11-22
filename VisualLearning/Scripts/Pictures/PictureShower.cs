using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Zenject;

public class PictureShower : MonoBehaviour
{
	[SerializeField] private Image _image;
	[SerializeField] private float _smoothFade;
	[SerializeField] private bool _switch;
	private UserInterfaceHandler _ui;

	[Inject]
	public void Init(UserInterfaceHandler ui)
	{
		_ui = ui;
	}
	public void SetPicture(Picture picture)
	{

		StartCoroutine(Fader(picture));

	}
	private IEnumerator Fader(Picture picture)
	{
		if (_switch)
		{
			_switch = false;
			_image.DOFade(0f, _smoothFade);
			yield return new WaitForSeconds(_smoothFade);
			_image.sprite = picture.Sprite;
			_ui.NameText.text = picture.Name.ToString();

			_image.DOFade(1f, _smoothFade);
			yield return new WaitForSeconds(_smoothFade);
			_switch = true;
		}
		
	}
}
