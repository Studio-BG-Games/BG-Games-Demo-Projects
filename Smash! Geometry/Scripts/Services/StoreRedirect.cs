using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreRedirect : MonoBehaviour
{
    public void OpenLink()
    {
        Application.OpenURL("market://details?id=com.smash.geometry");
    }
}
