using System;

namespace Gameplay.GameSceneScript
{
    public class FakeGold : IGold
    {
        public int Current => -5;
        public event Action<int> Update;
        public void Add(int count) { }

        public bool TryRemove(int count) => true;
    }
}