using System;
using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
	private Settings _settings;

	[Inject]
	public void Init(Settings settings)
	{
		_settings = settings;
	}
    public override void InstallBindings()
	{
		BindingLoaderPicture();
		BindingSwitchButtons();
	}

	private void BindingSwitchButtons()
	{
		var UserInterfaces = Container.InstantiatePrefabForComponent<UserInterfaceHandler>(_settings.UI);
		Container
			.Bind<UserInterfaceHandler>()
			.FromInstance(UserInterfaces)
			.AsSingle();
		Container
			.Bind<SwitchButton[]>()
			.FromInstance(_settings.ButtonSwitch)
			.AsSingle();

	}

	private void BindingLoaderPicture()
	{
		Container
			.Bind<ILoaderPictures>()
			.To<LoadPictures>()
			.AsSingle()
			.NonLazy();
	}

	[Serializable]
	public class Settings
	{
		[SerializeField] private UserInterfaceHandler _ui;
		public UserInterfaceHandler UI{ get{ return _ui; } }

		public SwitchButton[] ButtonSwitch{ get{ return _ui.SwitchButton; } }
	}
}