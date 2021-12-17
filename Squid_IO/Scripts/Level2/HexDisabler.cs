using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexDisabler : MonoBehaviour
{
    public bool destroying = false;
    Coroutine thisC;

    private void OnCollisionEnter(Collision collision)
    {
        if (thisC == null)
        { 
            thisC = StartCoroutine(Delayed(1f));
            GetComponent<MeshRenderer>().material.DOColor(Color.white, 1f);
        }
    }

    IEnumerator Delayed(float time)
    {
        destroying = true;
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
