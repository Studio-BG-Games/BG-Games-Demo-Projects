using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpSwithcer : MonoBehaviour
{
    [SerializeField] RectTransform joystick;
    [SerializeField] GameObject jumpButton;

    bool autojump = true;

    public void Toggle()
    {
        if (autojump)
        {
            autojump = false;
            jumpButton.SetActive(true);
            joystick.anchoredPosition = new Vector2(300, 230);
            joystick.localScale = new Vector3(1.5f, 1.5f, 1);
        }
        else
        {
            autojump = true;
            jumpButton.SetActive(false);
            joystick.anchoredPosition = new Vector2(0, 300);
            joystick.localScale = new Vector3(2f, 2f, 1);
        }
    }
}
