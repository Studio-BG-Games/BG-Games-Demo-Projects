using System.Collections;
using System.Collections.Generic;
using Scripts.UISystem;
using UnityEngine;
using Scripts;

public class LoginWindow : WindowController
{
    public void OnLeftButton()
    {
        WindowsManager.Instance.CreateWindow<SettingsWindow>();
        CloseWindow();
    }

    public void OnEnterButton()
    {
        PlayerPrefs.SetString("Login", "true");
        WindowsManager.Instance.CreateWindow<AccountWindow>();
        CloseWindow();
    }

}
