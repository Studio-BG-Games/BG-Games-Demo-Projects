using System.Collections;
using System.Collections.Generic;
using Scripts.Serializables.Story;
using Scripts.Managers;
using Scripts.UISystem;
using TMPro;
using UnityEngine;
using Scripts;

public class RelationsWindow : WindowController
{
    [SerializeField] Animation movePopUp;
    [SerializeField] TextMeshProUGUI textRelation;
    private bool _look = true;
    private string localizedName = "";

    void Update()
    {
        if (!movePopUp.isPlaying && !_look)
        {
            _look = true;
            CloseWindow();
        }
    }
    public void SetText(string keyName, int points)
    {
        var selectedStory = GameManager.Instance.selectedStory;        
        var generalName = selectedStory.general.Find(value => value.system == keyName);
        
        if(PlayerPrefs.GetString("language") == "rus")
        {
            localizedName = generalName.russian;
        }
        else if (PlayerPrefs.GetString("language") == "eng")
        {
            localizedName = generalName.english;
        }
        if(points > 0)
        {
            if(PlayerPrefs.GetString("language")=="rus")
                textRelation.text = $"Ваши отношения с \n{localizedName} улучшились";
            else if(PlayerPrefs.GetString("language")=="eng")
                textRelation.text = $"Your relationship with \n{localizedName} has improved";
        }
        else
        {
            if(PlayerPrefs.GetString("language")=="rus")
                textRelation.text = $"Ваши отношения с \n{localizedName} ухудшились";
            else if(PlayerPrefs.GetString("language")=="eng")
                textRelation.text = $"Your relationship with \n{localizedName} has deteriorated";
        }
        movePopUp.Play();
        _look = false;
    }
}
