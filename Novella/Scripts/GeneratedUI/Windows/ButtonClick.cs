using Scripts.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonClick : MonoBehaviour
{
    public void OnClick()
    {
        if (GameManager.Instance.soundManager.CheckAudioSource())
            GameManager.Instance.soundManager.PlayClick();
    }

}
