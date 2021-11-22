using System;
using Factories;
using Mechanics.GameLevel.Stages.BossStagePart.Interfaces;
using UnityEngine;

namespace Mechanics.GameLevel.Stages
{
    public class AgregatorPointerReciver : MonoBehaviour
    {
        private IPointerReciver[] _recivers;

        private void Awake() => _recivers = GetComponentsInChildren<IPointerReciver>();

        public void OffAll() => ChangeActive(false);

        public void OnAll() => ChangeActive(true);

        private void ChangeActive(bool toActive)
        {
            foreach (var reciver in _recivers) if (toActive) reciver.On(); else reciver.Off();
        }
    }
}