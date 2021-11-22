using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloon : MonoBehaviour
{
    [SerializeField] private Transform _pointBalloonTransform;
    private bool _isUsed;

    private void FixedUpdate()
    {
        if(_isUsed)
            transform.position = _pointBalloonTransform.position;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerController>().OnStartBallonMove();
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<Animator>().enabled = false;
            _isUsed = true;
        }
    }

    public void StopUsing()
    {
        _isUsed = false;
    }
}
