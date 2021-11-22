using System.Collections;
using System.Collections.Generic;
using Scripts.Serializables.Story;
using Scripts.UISystem;
using TMPro;
using UnityEngine;
using Scripts;

public class ReputationWindow : WindowController
{
    [SerializeField] Animation movePopUp;
    [SerializeField] TextMeshProUGUI textReputation;
    private bool _look = true;
    void Update()
    {
        if (!movePopUp.isPlaying && !_look)
        {
            _look = true;
            CloseWindow();
        }
    }
    public void SetText(int index)
    {
        if(index < 0)
            if(PlayerPrefs.GetString("language")=="rus")
                textReputation.text = $"{index} Репутации";
            else if (PlayerPrefs.GetString("language")=="eng")
                textReputation.text = $"{index} Reputation";
        if (index > 0)
            if(PlayerPrefs.GetString("language")=="rus")
                textReputation.text = $"+{index} Репутации";
            else if (PlayerPrefs.GetString("language")=="eng")
                textReputation.text = $"+{index} Reputation";

        movePopUp.Play();
        _look = false;
    }
}
