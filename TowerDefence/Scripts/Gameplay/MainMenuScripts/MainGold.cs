using System;
using Plugins.DIContainer;
using UnityEngine;

namespace Gameplay.UI.Menu
{
    public class MainGold : MonoBehaviour
    {
        public event Action<int> NewValue;
        
        [DI]private DataGameMono _dataGame;

        public int Current => _dataGame.CurrentGold;

        public bool TryRemove(int value)
        {
            if (value < 0)
                value = 0;
            if (value > Current)
                return false;
            _dataGame.SetGoldTo(Current-value);
            NewValue?.Invoke(Current);
            return true;
        }

        [ContextMenu("Test add 50")]
        private void TestAdd() => Add(50);
        
        public void Add(int value)
        {
            if(value<0)
                value=0;
            _dataGame.SetGoldTo(Current+value);
            NewValue?.Invoke(Current);
        }
    }
}