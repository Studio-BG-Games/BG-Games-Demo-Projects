using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cameratrigger : MonoBehaviour
{
    [SerializeField]
    Cinemachine.CinemachineFreeLook cinema;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            AnimateRotation();
        }
    }

    void AnimateRotation()
    {
        cinema.m_Heading.m_Bias += 0.1f;
        if (cinema.m_Heading.m_Bias >= 90f)
            return;
        Invoke("AnimateRotation", 0.001f);
    }
}
