using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] float timeToFinish;
    [SerializeField] Text text;

    private void Start()
    {
        StartCoroutine(StartTimer());
    }

    IEnumerator StartTimer()
    {
        while (timeToFinish !=0)
        {
            yield return new WaitForSeconds(1f);
            text.text = TimeSpan.FromSeconds(timeToFinish).ToString(@"mm\:ss");
            timeToFinish--;
        }
        yield return null;
    }
}
