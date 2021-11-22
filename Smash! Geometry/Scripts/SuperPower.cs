using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperPower : MonoBehaviour
{
    private GameObject player;
    public static GameObject SuperPowerInstance;

    private void Awake()
    {
        DoSingleton();
        player = GameObject.Find("Player");
        GameController.gameController.OnDeath += OnDeath;
       // CalculatePosition();
    }

    private void DoSingleton()
    {
        if (SuperPowerInstance == null)
        {
            SuperPowerInstance = transform.parent.gameObject;
        }
        else
            Destroy(transform.parent.gameObject);
    }

    private void OnDestroy()
    {
        GameController.gameController.OnDeath -= OnDeath;

    }

    void OnDeath()
    {
        Destroy(transform.parent.gameObject);
    }


}
