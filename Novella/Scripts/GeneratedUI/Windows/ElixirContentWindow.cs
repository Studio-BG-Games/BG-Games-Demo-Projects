using System.Collections;
using System.Collections.Generic;
using Scripts.UISystem;
using UnityEngine;
using TMPro;
using Scripts.Managers;
using GeneratedUI;
using UnityEngine.Purchasing;
using Scripts;

public class ElixirContentWindow : WindowController
{
    [SerializeField] private TextMeshProUGUI localizedDescription;
    string descriptionKey = "elixirs_inf";

    void Start()
    {
        var selectedStory = GameManager.Instance.selectedStory;
        var description = selectedStory.general.Find(value => value.system == descriptionKey);
        if (description.russian != null)
        {
            switch (PlayerPrefs.GetString("language"))
            {
                case "rus":
                    localizedDescription.text = description.russian;
                    break;
                case "eng":
                    localizedDescription.text = description.english;
                    break;
            }
        }
    }

    public void HandlePurchase(Product product)
    {
        ref var user = ref GameManager.Instance.userData;
        foreach (var item in product.definition.payouts)
        {
            Debug.Log(item.subtype + " " + item.quantity);
            if (item.subtype == "elixirs")
            {
                user.currencies.elixirs += (int) item.quantity;
            }
        }

        FirebaseManager.Instance.UpdateUserCurrencies();
        WindowsManager.Instance.SearchForWindow<ShopCurrencyWindow>()?.UpdateCurrencyBar();
        WindowsManager.Instance.SearchForScreen<MenuScreen>()?.UpdateCurrencyBar();
    }

    public void BuyElixir(int count)
    {
        ref var user = ref GameManager.Instance.userData;
        user.currencies.elixirs += count;
        FirebaseManager.Instance.UpdateUserCurrencies();
        //ES3.Save("user", GameManager.Instance.userData);
        WindowsManager.Instance.SearchForWindow<ShopCurrencyWindow>()?.UpdateCurrencyBar();
        WindowsManager.Instance.SearchForScreen<MenuScreen>()?.UpdateCurrencyBar();
    }
}