using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    [SerializeField] private GameObject _audioKillingObj;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            DontDestroyOnLoad(Instantiate(_audioKillingObj));
            collision.gameObject.GetComponent<PlayerController>().Death();
        }
    }
}
