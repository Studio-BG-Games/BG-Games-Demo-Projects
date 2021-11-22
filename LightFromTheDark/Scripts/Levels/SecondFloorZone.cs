using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondFloorZone : MonoBehaviour
{
    [SerializeField] private Animation _mainCameraAnim;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            _mainCameraAnim.Play("MoveUp");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            _mainCameraAnim.Play("MoveDown");
        }
    }
}
