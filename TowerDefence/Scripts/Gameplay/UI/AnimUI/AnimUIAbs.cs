using System;
using UnityEngine;

namespace Gameplay.UI.AnimUI
{
    public abstract class AnimUIAbs : MonoBehaviour
    {
        public abstract bool InProgress { get; }
        public abstract void On(Action callback);
        public abstract void Off(Action callback);
        public abstract void Complete();
    }
}