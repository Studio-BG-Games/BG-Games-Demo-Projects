using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    [SerializeField] Transform spawnPoint;
    [SerializeField]
    Cinemachine.CinemachineFreeLook cinema;

    void Update()
    {
        if (transform.position.y < -50f)
        {
            transform.position = spawnPoint.position;
            gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            cinema.m_Heading.m_Bias = 0;
        }
    }
}
