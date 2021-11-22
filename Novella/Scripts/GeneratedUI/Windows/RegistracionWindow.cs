using System.Collections;
using System.Collections.Generic;
using Scripts.UISystem;
using UnityEngine;
using Scripts;

public class RegistracionWindow : WindowController
{
    public void OnLeftButton()
    {    
        WindowsManager.Instance.CreateWindow<SettingsWindow>();
        CloseWindow();
    }

    public void OnRegisterButton()
    {     
        WindowsManager.Instance.CreateWindow<ConfirmEmailWindow>();
        CloseWindow();
    }
}
