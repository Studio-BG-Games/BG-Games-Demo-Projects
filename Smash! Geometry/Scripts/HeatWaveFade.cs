using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatWaveFade : MonoBehaviour
{
    [SerializeField] private float fadeOut;
    private Material material;
    private float Distortion = 20f;

    private void Start()
    {
        material = GetComponent<SpriteRenderer>().sharedMaterial;
    }

    void FixedUpdate()
    {
        if (Distortion > 0)
        {
            material.SetFloat("_BumpAmt", Distortion);
            Distortion -= fadeOut;
        }
    }
}
