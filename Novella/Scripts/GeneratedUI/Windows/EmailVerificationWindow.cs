using System;
using System.Collections.Generic;
using Scripts.Managers;
using Scripts.UISystem;
using TMPro;
using UnityEngine;
using Scripts;

namespace GeneratedUI
{
	public class EmailVerificationWindow : WindowController
	{
		[SerializeField] private List<TextMeshProUGUI> uiTexts;
		private List<string> _uiTextsRu = new List<string>
		{"На ваш e-mail направлено письмо для\nподтверждения. Следуйте инструкции в письме.","Спасибо, что вы с нами!",
			"Проверить подтверждение", "Отправить письмо еще раз"};
		private List<string> _uiTextsEng = new List<string>
		{"A confirmation email has been sent\n to your e-mail. Follow the instructions\nin the e-mail.","Thank you for being with us!",
			"Check confirmation", "Send e-mail again"};

		private void Start()
		{
			WindowsManager.Instance.SearchForWindow<LoadingWindow>()?.CloseWindow();
			switch (PlayerPrefs.GetString("language"))
			{
				case "eng":
					for (int i = 0; i < uiTexts.Count; i++)
					{
						uiTexts[i].text = _uiTextsEng[i];
					}
					break;
				case "rus":
					for (int i = 0; i < uiTexts.Count; i++)
					{
						uiTexts[i].text = _uiTextsRu[i];
					}
					break;
			}
		}

		private bool _initialFocus;
		private void OnApplicationFocus(bool focusStatus)
		{
			if (focusStatus)
			{
				CheckEmailVerified();
			}
			_initialFocus = _initialFocus || focusStatus;
		}

		public void CheckEmailVerified()
		{
			FirebaseManager.Instance.CheckEmailVerified(OnUserEmailVerified);
		}

		void OnUserEmailVerified(bool verified)
		{
			Debug.Log($"User is verified: {verified }");
			if (verified)
			{
				// TODO: Show "Congrats u verified ur email :D"
				var loginScreen = FindObjectOfType<LoginScreen>();
				if(loginScreen != null)
				{
					loginScreen.OnAuth(true);
					loginScreen.CloseWindow();
				}
				CloseWindow();				
			}
			else
			{
				// TODO: Show "Nope u did not yet verified your email :D"
			}
		}

		public void OpenRegisterScreen()
		{
			FirebaseManager.Instance.SignOut();
			WindowsManager.Instance.CreateScreen<RegisterScreen>();
			CloseWindow();
		}

		public void SendVerificationAgain()
		{
			FirebaseManager.Instance.SendVerificationEmail();
		}
	}
}