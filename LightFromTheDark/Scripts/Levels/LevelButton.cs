using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelButton : MonoBehaviour
{
    [HideInInspector] public bool IsActive;
    [SerializeField] private GameObject _movementObj;



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!IsActive)
        {
            IsActive = true;
            _movementObj.transform.position -= new Vector3(0, 0.3f, 0);
        }
    }
}
