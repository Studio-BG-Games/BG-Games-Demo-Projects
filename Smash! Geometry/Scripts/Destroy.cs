using System.Collections;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    [SerializeField] private float time;
    private void Start()
    {
        StartCoroutine(nameof(DestroyPart));
    }
    IEnumerator DestroyPart()
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
