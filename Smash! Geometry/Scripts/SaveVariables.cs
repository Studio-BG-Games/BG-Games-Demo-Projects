using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveVariables : MonoBehaviour
{
    public bool BannerLoaded;
    void Start()
    {
        DontDestroyOnLoad(this);
    }

}
