using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIEnabler : MonoBehaviour
{
    AILogic AI;
    Rigidbody rb;

    private void Start()
    {
        AI = GetComponent<AILogic>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (AI.enabled && Mathf.Abs(rb.velocity.y) > 2f)
        {
            AI.enabled = false;
        }
        else if (!AI.enabled)
        {
            AI.enabled = true;
        }
    }
}
