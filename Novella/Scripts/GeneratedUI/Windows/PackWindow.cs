using System.Collections;
using System.Collections.Generic;
using Scripts.UISystem;
using TMPro;
using UnityEngine;
using Scripts;

public class PackWindow : WindowController
{
    public static int pack;
    [SerializeField] private TextMeshProUGUI text;

    private void Start()
    {
        switch (PlayerPrefs.GetString("language"))
        {
            case "rus":
                text.text = "Коснитесь, чтобы продолжить";
                break;
            case "eng":
                text.text = "Tap to continue";
                break;
        }
    }
        public void OpenPackInfo()
    {
        WindowsManager.Instance.CreateWindow<PackInfoWindow>();
    }

    public void OnBuyButton(int choice)
    {
        pack = choice;
        WindowsManager.Instance.CreateWindow<ShopCurrencyWindow>();
        WindowsManager.Instance.CreateWindow<RouletteWindow>();
        CloseWindow();
        
    }
}
