using System.Collections;
using System.Collections.Generic;
using Scripts.UISystem;
using UnityEngine;
using Scripts.Managers;
using Scripts.Tools.Actions;
using TMPro;
using GeneratedUI;
using Scripts;

public class AdWindow : WindowController
{
    [SerializeField] private TextMeshProUGUI localizedDescription;
    [SerializeField] private TextMeshProUGUI localizedOkButton;
    public string descriptionKey = "ads_msg";
    public string okButtonKey ="ok";//kappa
    private void Start()
    {  
        var selectedStory = GameManager.Instance.selectedStory;        
        var description = selectedStory.general.Find(value => value.system == descriptionKey);
        var okButton = selectedStory.general.Find(value => value.system == okButtonKey);
        switch (PlayerPrefs.GetString("language"))
        {
            case "rus": 
                localizedDescription.text = description.russian;
                localizedOkButton.text = okButton.russian;
                break;
            case "eng":
                localizedDescription.text = description.english;
                localizedOkButton.text = okButton.russian;
                break;
        }
    }

    private System.Action callback;

    public void SetCloseCallback(System.Action pCallback)
    {
        callback = pCallback;
    }
    
    public void Close()
    {
        WindowsManager.Instance.SearchForScreen<StoryScreen>()?.ControlTouchPanel(true);
        CloseWindow();
        callback?.Invoke();
        callback = null;
    }
}
