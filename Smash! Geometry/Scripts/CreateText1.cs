using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateText1 : MonoBehaviour
{
    private Animation animation;
    [SerializeField] private GameObject text;
    [SerializeField] private GameObject UI;
    [SerializeField] private string achivemessage;
    void Start()
    {
        UI = GameObject.Find("UI");
        animation = gameObject.GetComponent<Animation>();
        GameObject AchiveText = new GameObject("Achivetext");
        AchiveText = text;
        AchiveText.gameObject.SetActive(true);
        text.GetComponent<Text>().text = achivemessage;
        transform.SetParent(UI.transform);
        AchiveText.transform.SetParent(transform);
    }
}
