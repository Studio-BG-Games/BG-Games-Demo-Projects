using System.Text.RegularExpressions;
using Scripts.Managers;
using Scripts.Tools.Actions;
using Scripts.UISystem;
using TMPro;
using UnityEngine;
using Scripts.Serializables.User;
using Scripts;

namespace GeneratedUI
{
	public class LoginScreen : WindowController
	{
#pragma warning disable 0649
		[SerializeField] private TMP_InputField email;
		[SerializeField] private TMP_InputField password;
#pragma warning restore 0649

		private string _currentSound;

		public void Login()
		{
			if (InputsValid())
			{
				FirebaseManager.Instance.OnAuth += OnAuth;
				FirebaseManager.Instance.Login(email.text, password.text);
			}
		}

		private void Start()
		{

			if (!GameManager.Instance.soundManager.CheckAudioSource())
				_currentSound = GameManager.Instance.soundManager.PlaySounds(new string[1] { "1. Меню, гардероб" }, 1.5f, 1.5f);
		}


		private bool InputsValid()
		{
			// Validate Email Shape
			Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
			Match match = regex.Match(email.text);
			bool emailValid = match.Success;
			// Validate Password
			bool passwordValid = password.text.Length >= 8;

			if (!emailValid && !passwordValid)
			{
				WindowsManager.Instance.CreateWindow<EmailAndPasswordErrorWindow>();
			}
			else if (!emailValid)
			{
				WindowsManager.Instance.CreateWindow<EmailErrorWindow>();
			}
			else if (!passwordValid)
			{
				WindowsManager.Instance.CreateWindow<PasswordErrorWindow>();
			}

			return emailValid && passwordValid;
		}

		public void OnAuth(bool success)
		{
			FirebaseManager.Instance.OnAuth -= OnAuth;
			if (success)
			{
				WindowsManager.Instance.CreateWindow<LoadingWindow>();
				WindowsManager.Instance.CreateScreen<MenuScreen>();

				ActionSequence loginActionSequence = new ActionSequence("Login Action Sequence", GameManager.Instance, new Action[]
				{
					new WaitForUserData { name = "Loading User Data"},
					new LoadStories { name = "Loading Stories"}

				});
				loginActionSequence.OnSequenceComplete += LoadUserData;
				loginActionSequence.StartSequence();				
			}
			else
			{
				email.text = null;
				password.text = null;
				Debug.Log("Incorrect Password");
				WindowsManager.Instance.CreateWindow<EmailAndPasswordErrorWindow>();
			}
		}
		private void LoadUserData()
		{		
			if (ES3.KeyExists("user"))
			{
				//GameManager.Instance.userData = (User)ES3.Load("user");
			}				
		}
		public void OpenRegistrationScreen()
		{
			WindowsManager.Instance.CreateScreen<RegisterScreen>();
			CloseWindow();
		}
	}
}