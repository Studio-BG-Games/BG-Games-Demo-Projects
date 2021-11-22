using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChainButton : MonoBehaviour
{
    public Button button;
    public TextMeshProUGUI chainDescription;
    public bool isComplete;

    private void Start()
    {
        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        if (!isComplete) isComplete = true;
        button.interactable = false;
    }
}
