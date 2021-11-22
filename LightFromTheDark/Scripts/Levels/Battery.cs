using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery : MonoBehaviour
{
    //Время пополнения энергии фонарику
    [SerializeField] private float _energyTime;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Flashlight flashlight = collision.gameObject.GetComponent<PlayerController>().Flashlight;
            if (flashlight != null)
            {
                flashlight.AddEnergy(_energyTime);
                Destroy(gameObject);
            }
        }
    }
}
