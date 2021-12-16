using System;
using Plugins.HabObject;
using UnityEngine;

namespace Gameplay
{
    [RequireComponent(typeof(Collider))]
    public abstract class SelectorHabObject : MonoBehaviour
    {
        public abstract HabObject Get();

        private void OnValidate()
        {
            gameObject.layer = 9;
        }
    }
}