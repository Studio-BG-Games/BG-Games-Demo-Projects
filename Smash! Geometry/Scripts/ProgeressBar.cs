using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgeressBar : MonoBehaviour
{
    private Slider slider;
    private float progressValue;
    private GameObject fillComponent;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject firstFigure;
    private int index = 1;
    public int NumberOfFigures = 0;

    void Awake()
    {
        slider = gameObject.GetComponent<Slider>();
        fillComponent = transform.GetChild(1).transform.GetChild(0).gameObject;
        player = GameObject.Find("Player");
        NumberOfFigures = firstFigure.GetComponent<Border>().NumberOfFigures+1;
       // progressValue = 1f / player.GetComponent<Player>().numberOfFigures;
        //slider.value = progressValue;


    }

    public void ChangeProgress()
    {
        progressValue += 1f/ NumberOfFigures;
        slider.value = progressValue;
        CheckIfComlete();
    }

    private void CheckIfComlete()
    {
        if (index == 8)
            progressValue = 1;
        
        if (progressValue >= 0.98 && index != 7)
        {
            NumberOfFigures = player.GetComponent<Player>()._borders[index].GetComponent<Border>().NumberOfFigures;
            progressValue = 0;
            slider.value = progressValue;
            fillComponent.GetComponent<Image>().color = player.GetComponent<Player>()._borders[index].transform.GetChild(1).transform.GetChild(0).GetComponent<SpriteRenderer>().color;
            index++;
        }
        if (index > 7)
            index = 7;
    }

   
}
