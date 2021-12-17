using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] Transform variant1;
    [SerializeField] Transform variant2;

    Transform nextCheckpoint;
    int value;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "AI")
        {
            if (variant2 != null)
            {
                value = Random.Range(0, 2);
                switch (value)
                {
                    case 0: 
                        nextCheckpoint = variant1;
                        break;
                    case 1: 
                        nextCheckpoint = variant2;
                        break;
                    default:
                        break;
                }
            }
            else
            {
                nextCheckpoint = variant1;
            }

            other.GetComponent<NavMeshAgent>().SetDestination(nextCheckpoint.position);
        }
    }
}
