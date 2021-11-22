using System.Collections;
using System.Collections.Generic;
using Scripts.Managers;
using Scripts.UISystem;
using GeneratedUI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RouletteWindow : PackWindow
{
    [SerializeField] private Sprite[] rubies = new Sprite[3];
    [SerializeField] private Image defaultRuby;
    [SerializeField] private GameObject amountOfRuby;
    [SerializeField] private GameObject takeButton;
    [SerializeField] private TextMeshProUGUI takeButtonText;
    [SerializeField] Animation spinRoulette, textFade;

    private bool _look;

    private int[] silverPrizes = new int[] { 20, 23, 28, 40, 55 };
    private int[] goldPrizes = new int[] { 45, 50, 55, 70, 100 };
    private int[] diamondPrizes = new int[] { 65, 70, 75, 100, 1000 };

    private void Start()
    {
        switch (PlayerPrefs.GetString("language"))
        {
            case "eng":
                takeButtonText.text = "Get";
                break;
            case "rus":
                takeButtonText.text = "Забрать";
                break;
        }

        switch (pack)
        {
            case 1:
                ChoiceOfChance(silverPrizes);
                break;

            case 2:
                ChoiceOfChance(goldPrizes);
                break;

            case 3:
                ChoiceOfChance(diamondPrizes);
                break;
        }
    }

    private void Update()
    {
        if (!spinRoulette.isPlaying && !_look)
        {
            _look = true;
            textFade.Play();
            takeButton.GetComponent<Button>().interactable = true;
        }
    }

    private void ChoiceOfChance(int[] prizes)
    {
        ref var user = ref GameManager.Instance.userData;
        float chance = Random.Range(0f, 100f);

        if (chance >= 0f && chance <= 60f)
        {
            amountOfRuby.GetComponent<TextMeshProUGUI>().text = "+" + prizes[0].ToString();
            user.currencies.cash += prizes[0];
        }
        else if (chance >= 61f && chance <= 90f)
        {
            amountOfRuby.GetComponent<TextMeshProUGUI>().text = "+" + prizes[1].ToString();
            defaultRuby.sprite = rubies[0];
            user.currencies.cash += prizes[1];
        }
        else if (chance >= 91f && chance <= 99f)
        {
            amountOfRuby.GetComponent<TextMeshProUGUI>().text = "+" + prizes[2].ToString();
            defaultRuby.sprite = rubies[0];
            user.currencies.cash += prizes[2];
        }
        else if (chance >= 99.1f && chance <= 99.9f)
        {
            amountOfRuby.GetComponent<TextMeshProUGUI>().text = "+" + prizes[3].ToString();
            defaultRuby.sprite = rubies[1];
            user.currencies.cash += prizes[3];
        }
        else
        {
            amountOfRuby.GetComponent<TextMeshProUGUI>().text = "+" + prizes[4].ToString();
            defaultRuby.sprite = rubies[2];
            user.currencies.cash += prizes[4];
        }

        FirebaseManager.Instance.UpdateUserCurrencies();
        //ES3.Save("user", GameManager.Instance.userData);      
    }

    public void OnTakeButton()
    {
        WindowsManager.Instance.SearchForScreen<MenuScreen>()?.UpdateCurrencyBar();
        WindowsManager.Instance.CloseAllWindows();
    }
}
