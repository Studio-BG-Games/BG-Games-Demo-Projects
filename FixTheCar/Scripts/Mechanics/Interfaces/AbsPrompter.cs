using System;
using UnityEngine;

namespace Mechanics.Interfaces
{
    public abstract class AbsPrompter : MonoBehaviour
    {
        public abstract void Unhide(Action action = null);

        public abstract void Say(string message, Action callback = null);

        public abstract void Hide(Action action = null);
    }
}