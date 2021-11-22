using System;
using TMPro;
using UnityEngine;

namespace Mechanics.Prompters.Interfaces
{
    public abstract class AbsDelayPrometed : ScriptableObject
    {
        public abstract void Activated(Action callback);
    }
}