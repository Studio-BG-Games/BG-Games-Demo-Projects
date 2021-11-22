using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryCover : MonoBehaviour
{
    public Image cover;
    [SerializeField] private Sprite rusCover, engCover;
    private void Start()
    {
        switch (PlayerPrefs.GetString("language"))
        {
            case "rus":
                cover.sprite = rusCover;
                break;
            case "eng":
                cover.sprite = engCover;
                break;
        }
    }

}
