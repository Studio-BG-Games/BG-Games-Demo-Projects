using System.Collections;
using UnityEngine;

public class Vibration : MonoBehaviour
{
    private bool isVibration = false;


    void Update()
    {
        if(isVibration == true)
            Handheld.Vibrate();
    }

    public void PlayVibration(float time)
    {
        StartCoroutine(VibrationTime(time));
    }

    public void PlayVibration(bool isPressed)
    {
        isVibration = isPressed;
    }

    public void PlayVibration()
    {
        Handheld.Vibrate();
    }

    private IEnumerator VibrationTime(float time)
    {
        isVibration = true;
        yield return new WaitForSecondsRealtime(time);
        isVibration = false;
    }
}
