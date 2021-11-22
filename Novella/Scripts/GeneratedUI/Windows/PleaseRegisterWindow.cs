using System.Collections;
using System.Collections.Generic;
using Scripts.UISystem;
using UnityEngine;
using TMPro;
using Scripts;

public class PleaseRegisterWindow : WindowController
{
    [SerializeField] private TextMeshProUGUI description, okButton;
    void Start()
    {
        switch(PlayerPrefs.GetString("language"))
        {
			case "rus":
				description.text = "Здравствуй, дорогой игрок!\nПрежде, чем начать, зарегистрируйтесь,\nпожалуйста. Вы можете сделать это в настройках       и мы начислим вам 2";
				okButton.text = "Ок";
                break;
			case "eng":
				description.text = "Hello dear player!\nBefore you start, register,\nplease. You can do this in the settings       we will credit you 2";
                okButton.text = "Ok";
				break;
		}
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
