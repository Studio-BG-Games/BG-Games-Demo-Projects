using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextInputField : MonoBehaviour
{
    public InputField nextField;


    void Update()
    {
        if (GetComponent<InputField>().isFocused && Input.GetKeyDown(KeyCode.Tab))
        {
            nextField?.ActivateInputField();
        }
    }
}