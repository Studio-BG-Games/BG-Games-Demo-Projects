using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsideFigure : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("IN1");

        if ((collision.gameObject.name == "breaked(Clone)"))
        {
            Debug.Log("IN2");
            Destroy(collision.gameObject);
        }
    }
}
