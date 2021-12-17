using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotEnabler : MonoBehaviour
{
    [SerializeField] List<GameObject> bots;

    void Start()
    {
        float amount = MenuScript.botAmount;

        for (int i = 0; i < 19; i++)
        {
            if (i >= amount)
            {
                bots[i].SetActive(false);
            }
        }
    }
}
