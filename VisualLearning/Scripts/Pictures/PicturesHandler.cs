using UnityEngine;
using System.Linq;
using Zenject;
using System;

public class PicturesHandler : MonoBehaviour
{
	private ILoaderPictures _loader;
	private PictureShower _pictureShower;
	private SwitchButton[] _buttonsSwitch;
	private AudioHandler _audioHandler;
	private int _idSelected;

	[Inject]
	public void Init(ILoaderPictures loader, PictureShower pictureShower, UserInterfaceHandler ui, AudioHandler audioHandler)
	{
		_loader = loader;
		_pictureShower = pictureShower;

		_buttonsSwitch = ui.SwitchButton;
		_pictureShower.SetPicture(_loader.GetPictures()[_idSelected]);
		_buttonsSwitch.ToList().ForEach(b => b.OnSwitchPicture += SwitchPicture);

		_audioHandler = audioHandler;
	}
	private void Start()
	{
		_idSelected = default;
	}
	private void OnDestroy()
	{
		_buttonsSwitch.ToList().ForEach(b => b.OnSwitchPicture -= SwitchPicture);
	}

	private void SwitchPicture(bool vector)
	{
		if(vector)
		{
			_idSelected++;
			if (_idSelected >= _loader.GetPictures().Count) 
			{
				_idSelected = _loader.GetPictures().Count;
				return;
			}

			_pictureShower.SetPicture(_loader.GetPictures()[_idSelected]);
		}
		else
		{
			_idSelected--;
			if (_idSelected < 0)
			{
				_idSelected = default;
				return;
			}

			_pictureShower.SetPicture(_loader.GetPictures()[_idSelected]);
		}
	}
}
