using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CameraRotation : MonoBehaviour
{
    [SerializeField] Cinemachine.CinemachineFreeLook cinema;

    public void OnSliderChanged(float value)
    {
        cinema.m_YAxis.Value = value;
    }
}
