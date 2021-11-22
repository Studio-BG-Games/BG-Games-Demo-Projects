using System;
using System.Collections;
using System.Collections.Generic;
using Scripts.Managers;
using Scripts.Serializables.Story;
using Scripts.UISystem;
using GeneratedUI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionButton : MonoBehaviour
{
    public event Action<int> OnOptionSelected;
    [SerializeField] Button button;
    [SerializeField] TextMeshProUGUI optionDescriptionText, optionPaidDescriptionText;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private Sprite paidOptionSprite;
    [SerializeField] private GameObject priceStuff;    
    
    
    private int _optionIndex;
    private int _price;
    private int _points;
    private bool _qlinedIsPaid; 
     

    //private PlayerInformation PlayerInformation => GameManager.Instance.playerInformation;

    private void Start()
    {
        button.onClick.AddListener(OnButtonClick);   
    }
    
    private void OnButtonClick()
    {         
        if(_price > 0 && _qlinedIsPaid)
        {
            Debug.Log("Qline is free cuz it was paid earlier");
            OnOptionSelected?.Invoke(_optionIndex);
            return;
        }
        ref var user = ref GameManager.Instance.userData;    
        var selectedStory = GameManager.Instance.selectedStory;   
        if(Int32.TryParse(selectedStory.id, out var storyId))
        {
            if(_price > 0 && user.progress[storyId].isElixirOn==true)
            {
                Debug.Log("Qline is free cuz the elixir is used");
                OnOptionSelected?.Invoke(_optionIndex);
                return;
            }
        }
        else
        {
            Debug.LogWarning($"<color=red>Can't parse selectedStory.id: {selectedStory.id} to Int32</color>");            
        }   
        if (user.currencies.cash >= _price)//PlayerInformation.cash
        {
            Debug.Log("Qline is paid right now");
            user.currencies.cash -= _price;
            FirebaseManager.Instance.UpdateUserData();
            //ES3.Save("user", GameManager.Instance.userData);
            //PlayerInformation.cash -= _price;
            OnOptionSelected?.Invoke(_optionIndex);
        }
        else
        {
            WindowsManager.Instance.CreateWindow<NoMoneyPopupWindow>();
        }
    }
    
    public void SetOptionStuff(Option option, int index)
    {
        var user = GameManager.Instance.userData;
        // Debug.Log($"Russian: {option.text} >>> English: {option.english}");
        switch (PlayerPrefs.GetString("language"))
        {
            case "rus":
                optionDescriptionText.SetText(option.text);
                optionPaidDescriptionText.SetText(option.text);
                break;
            case "eng":
                optionDescriptionText.SetText(option.english);
                optionPaidDescriptionText.SetText(option.english);
                break;
        }
        _optionIndex = index;
        _price = option.price;
        _points = option.points;
        
        if(_price > 0)
        {
            var selectedStory = GameManager.Instance.selectedStory;
            var progress = user.progress.Find(value => value.storyId == selectedStory.id);
            string optionQline = string.Join("", option.qline);           
            
            if(progress.paidQlines.Contains(optionQline))
            {   
                _qlinedIsPaid=true;
                optionPaidDescriptionText.SetText("");
                return;
            }

            else if(_price > 0 && progress.isElixirOn==true)
            {
                optionDescriptionText.SetText("");//Тут оставлю optionDescriptionText, чтобы у людей был шанс не прослакать платный выбор под эликсиром
                priceStuff.SetActive(true);
                button.image.sprite = paidOptionSprite;
                priceText.SetText("0");
                return;    
            }
            else
            {
                optionDescriptionText.SetText("");// optionDescriptionText
                priceStuff.SetActive(true);
                button.image.sprite = paidOptionSprite;
                priceText.SetText($"{_price}");
            } 
        }        

        else if (_price <= 0)
        {
            optionPaidDescriptionText.SetText("");
            return;
        }       


    }
}