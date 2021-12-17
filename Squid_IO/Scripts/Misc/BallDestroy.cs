using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallDestroy : MonoBehaviour
{
    [SerializeField] float destroyLevel = -50;

    private void Update()
    {
        if (transform.position.y <= -50)
            Destroy(gameObject);
    }
}
