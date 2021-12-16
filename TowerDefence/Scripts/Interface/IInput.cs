using System;
using UnityEngine;

namespace Interface
{
    public interface IInput
    {
        event Action<Vector3> MoveCameraAtDiraction;
        event Action<float> ChangeFov;
        bool TryRaycstFromPoisitonInput(float lenght, LayerMask laeyrMask, out RaycastHit raycastHit);
    }
}