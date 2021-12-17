using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLoose : MonoBehaviour
{
    void Update()
    {
        if (transform.position.y < -35)
        {
            //Loose
            SceneManager.LoadScene("MainMenu");
        }
    }
}
