using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    //Убить игрока?
    [HideInInspector] public bool IsKillPlayer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if (IsKillPlayer)
                collision.gameObject.GetComponent<PlayerController>().Death();
            else
                Destroy(gameObject);
        }
    }
}
