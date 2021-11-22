using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowTrigger : MonoBehaviour
{
    private GameController EventSystem;
    private void Start()
    {
        EventSystem = GameObject.Find("EventSystem").gameObject.GetComponent<GameController>();
    }
                
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player")
            EventSystem._currentBorder.gameObject.GetComponent<Border>().borderDestring = true;
    }
}
