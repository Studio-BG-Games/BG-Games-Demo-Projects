using UnityEngine;

namespace Mechanics.GameLevel.Stages
{
    public abstract class State : MonoBehaviour
    {
        private void Awake() => Off();

        public abstract void On();
        public abstract void Off();

        public virtual void Step()
        {
        }

        public abstract State TransitToOrNull();
    }
}