using System;
using UnityEngine;

namespace Mechanics.GameLevel.Stages.BossStagePart.Interfaces
{
    public interface IBossPart
    {
        event Action Cleaned;
        public Transform Transform { get; }
        public bool IsClear();
        public void MakeDirty();
        public void MakeClear();
    }
}