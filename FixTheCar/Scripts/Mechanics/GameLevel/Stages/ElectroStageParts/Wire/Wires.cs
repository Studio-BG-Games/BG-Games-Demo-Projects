using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Mechanics.GameLevel.Stages.ElectroStageParts.Wire
{
    public class Wires : MonoBehaviour
    {
        public event Action FixedAll;
        public event Action FixedOnePart;
        public event Action FaildSet;

        public List<WirePartShell> WiresList => _partShell;

        [SerializeField] private List<WirePartShell> _partShell = new List<WirePartShell>();

        private List<WirePartShell> _breakeWire;
        
        public int MaxBreakePart => _partShell.Count;

        public void Break(int count)
        {
            List<WirePartShell> newList = new List<WirePartShell>();
            newList = _partShell.ToList();
            _breakeWire = new List<WirePartShell>();
            for (int i = 0; i < count; i++)
            {
                var element =  newList[Random.Range(0, newList.Count)];
                element.Break();
                newList.Remove(element);
                _breakeWire.Add(element);
                SubscribeToElement(element);
            }
        }

        private void SubscribeToElement(WirePartShell element)
        {
            element.Fixed += OnFixed;
            element.Failed += OnFaild;
        }

        private void OnFixed(WirePartShell shell)
        {
            _breakeWire.Remove(shell);
            shell.Fixed -= OnFixed;
            shell.Failed -= OnFaild;
            if(_breakeWire.Count == 0)
                FixedAll?.Invoke();
            else
                FixedOnePart?.Invoke();
        }

        private void OnFaild(WirePartShell shell)
        {
            FaildSet?.Invoke();
        }
    }
}