using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingShaft : MonoBehaviour
{
    [SerializeField] float force = 5f;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<Rigidbody>() != null)
        {
            other.gameObject.GetComponent<Rigidbody>().AddForce(transform.right * force, ForceMode.Acceleration);
        }
    }
}
