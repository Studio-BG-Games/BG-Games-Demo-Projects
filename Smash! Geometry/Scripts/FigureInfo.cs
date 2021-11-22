using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects", order = 1)]

public class FigureInfo : ScriptableObject
{
   
        [SerializeField] float colorPlus;
        [SerializeField] float colorMinus;

    public float ColorPlus => this.colorPlus;
    public float ColorMinus => this.colorMinus;

}
