using System.Collections;
using System.Collections.Generic;
using Scripts.Managers;
using Scripts.UISystem;
using GeneratedUI;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;
using Scripts;

public class CocktailContentWindow : WindowController
{
    [SerializeField] private Image offer;
    [SerializeField] private Sprite ruOffer;
    [SerializeField] private Sprite engOffer;
    private void Start()
    {
        switch (PlayerPrefs.GetString("language"))
        {
            case "rus":
                offer.sprite = ruOffer;
                break;
            case "eng":
                offer.sprite = engOffer;
                break;
        }
    }

    public void HandlePurchase(Product product)
    {
        ref var user = ref GameManager.Instance.userData;
        foreach (var item in product.definition.payouts)
        {
            Debug.Log(item.subtype + " " + item.quantity);
            if (item.subtype == "cocktails")
            {
                user.currencies.cocktails += (int)item.quantity;
            }
            else if (item.subtype == "rubies")
            {
                user.currencies.cash += (int)item.quantity;
            }
        }
        FirebaseManager.Instance.UpdateUserCurrencies();
        WindowsManager.Instance.SearchForWindow<ShopCurrencyWindow>()?.UpdateCurrencyBar();
        WindowsManager.Instance.SearchForScreen<MenuScreen>()?.UpdateCurrencyBar();
    }
    
    public void BuyCocktail(int count)
    {
        ref var user = ref GameManager.Instance.userData;
        user.currencies.cocktails += count;
        FirebaseManager.Instance.UpdateUserCurrencies();
        //ES3.Save("user", GameManager.Instance.userData);
        WindowsManager.Instance.SearchForWindow<ShopCurrencyWindow>()?.UpdateCurrencyBar();
        WindowsManager.Instance.SearchForScreen<MenuScreen>()?.UpdateCurrencyBar();
    }
    
    public void BuySpecialPack()
    {
        ref var user = ref GameManager.Instance.userData;
        user.currencies.cocktails += 7;
        user.currencies.cash += 45;
        FirebaseManager.Instance.UpdateUserCurrencies();
        //ES3.Save("user", GameManager.Instance.userData);
        WindowsManager.Instance.SearchForWindow<ShopCurrencyWindow>()?.UpdateCurrencyBar();
        WindowsManager.Instance.SearchForScreen<MenuScreen>()?.UpdateCurrencyBar();
    }
}
