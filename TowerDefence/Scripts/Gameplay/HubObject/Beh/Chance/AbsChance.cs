using UnityEngine;

namespace Gameplay.Units.Beh
{
    public abstract class AbsChance<T> : MonoBehaviour
    {
        [Range(0,1f)][SerializeField] private float _chanceToSucces;
        protected abstract T Succes { get; }
        protected abstract T Lose { get; }

        public T Check() => (Random.Range(0, 1f) < _chanceToSucces) ? Succes : Lose;
    }
}