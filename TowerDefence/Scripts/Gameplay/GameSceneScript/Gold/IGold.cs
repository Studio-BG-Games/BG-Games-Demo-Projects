using System;

namespace Gameplay.GameSceneScript
{
    public interface IGold
    {
        public int Current { get; }
        public event Action<int> Update;
        public void Add(int count);
        public bool TryRemove(int count);
    }
}