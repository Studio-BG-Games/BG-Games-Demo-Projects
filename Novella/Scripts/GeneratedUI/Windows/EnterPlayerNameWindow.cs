using System;
using Scripts.UISystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Scripts.Managers;
using Scripts;

public class EnterPlayerNameWindow : WindowController
{
    public event  Action <string> OnComplete;
    [SerializeField] private TMP_InputField nameField;
    [SerializeField] private Button readyButton;
    [SerializeField] private TextMeshProUGUI localizedDescription;
    [SerializeField] private TextMeshProUGUI localizedOkButton;
    [SerializeField] private TextMeshProUGUI localizedPlaceholder;
    public string descriptionKey = "enter_your_name";
    public string okButtonKey ="ok";//kappa      
    
    private void Start()
    {
        readyButton.onClick.AddListener(OnEnterNameComplete);
        var selectedStory = GameManager.Instance.selectedStory;        
        var description = selectedStory.general.Find(value => value.system == descriptionKey);
        var okButton = selectedStory.general.Find(value => value.system == okButtonKey);
        switch (PlayerPrefs.GetString("language"))
        {
            case "rus": 
                localizedDescription.text = description.russian;
                localizedOkButton.text = okButton.russian;
                localizedPlaceholder.text = "Стелла";
                break;
            case "eng":
                localizedDescription.text = description.english;
                localizedOkButton.text = okButton.russian;
                localizedPlaceholder.text = "Stella";
                break;
        }
    }
    
    private void OnEnterNameComplete()
    {
        string name;
        if (nameField.text == "")
        {
            if(PlayerPrefs.GetString("language")=="rus")
                name = "Стелла";
            else
                name = "Stella";            
        } 
        else name = nameField.text;
        OnComplete?.Invoke(name);
        CloseWindow();
    }
}
