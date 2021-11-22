using System.Collections.Generic;
using System.Text.RegularExpressions;
using Scripts.Managers;
using Scripts.Tools.Actions;
using Scripts.UISystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Scripts;

namespace GeneratedUI
{
	public class RegisterScreen : WindowController
	{
		[SerializeField] private TMP_InputField email;
		[SerializeField] private TMP_InputField password;
		[SerializeField] private TMP_InputField username;
		[SerializeField] private List<TextMeshProUGUI> uiTexts;
		[SerializeField] private Button registerButton;

		private List<string> _uiTextsEng = new List<string>
	    {"Fill in the data", "Password", "Name", "Registration"};

		private void Start()
		{
			switch (PlayerPrefs.GetString("language"))
			{
				case "eng":
					for (int i = 0; i < uiTexts.Count; i++)
					{
						uiTexts[i].text = _uiTextsEng[i];
					}
					break;
			}
		}
		public void Register()
		{
			registerButton.interactable = false;

			// TODO: ADD Loading Screen or Disable the button at least;
			if (InputsValid())
			{
				FirebaseManager.Instance.Register(email.text, password.text, username.text);
				FirebaseManager.Instance.OnRegister += OnRegistered;
			}
		}
		private bool InputsValid()
		{
			// Validate Email Shape
			Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
			Match match = regex.Match(email.text);
			bool emailValid = match.Success;
			// Validate Password
			bool passwordValid = password.text.Length >= 8;
			// Validate Username
			bool nameValid = username.text.Length > 0;


			if ((!emailValid && !passwordValid && !nameValid) ||
				(!emailValid && !passwordValid) ||
				(!emailValid && !nameValid) ||
				(!passwordValid && !nameValid))
			{
				WindowsManager.Instance.CreateWindow<ErrorWindow>();
			}
			else if (!emailValid)
			{
				WindowsManager.Instance.CreateWindow<EmailErrorWindow>();
			}
			else if (!passwordValid)
			{
				WindowsManager.Instance.CreateWindow<PasswordErrorWindow>();
			}
			else if (!nameValid)
			{
				WindowsManager.Instance.CreateWindow<UsernameErrorWindow>();
			}

			return emailValid && passwordValid && nameValid;
		}

		private void OnRegistered(bool success)
		{
			if (success)
			{
				// Overlay with Loading Window
				WindowsManager.Instance.CreateWindow<LoadingWindow>();
				// Open Menu Screen
				WindowsManager.Instance.CreateScreen<MenuScreen>();
				// Login User with Registered Creedentials
				FirebaseManager.Instance.Login(email.text, password.text);
				// Run Action Sequence to load User Data and Content
				ActionSequence registerActionSequence = new ActionSequence("Register Sequence", this, new Action[]
				{
					// Wait for Auth
					new WaitForAuth { name = "Waiting For Auth"},
					// Send Email Verification
					new SendEmailVerification { name = "Sent Email Verification"},
					// Load User Data
					new WaitForUserData { name = "Loading User Data"},
					//TODO: LOAD BUNDLES
					// Load User data and Stories
					new LoadStories { name = "Loading Stories"}
				});

				registerActionSequence.StartSequence();
			}
			else
			{
				WindowsManager.Instance.CreateWindow<AccountErrorWindow>();
				registerButton.interactable = true;
			}
		}
		public void OpenLoginScreen()
		{
			WindowsManager.Instance.CreateScreen<LoginScreen>();
			CloseWindow();
		}

		private void OnDestroy() 
		{
			FirebaseManager.Instance.OnRegister -= OnRegistered;
		}
	}
}