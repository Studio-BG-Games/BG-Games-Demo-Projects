using System;
using UnityEngine;

namespace Mechanics.GameLevel.Stages.ElectroStageParts.Wire
{
    public class WirePartShell : MonoBehaviour
    {
        public event Action<WirePartShell> Fixed;
        public event Action<WirePartShell> Failed;

        public WiresPart WiresPart => _wiresPart;
        
        [SerializeField] private WiresPart _wiresPart;

        private void Awake()
        {
            _wiresPart.Fixed += OnFixed;
            _wiresPart.Failed += OnFailed;
        }

        public void Break() => _wiresPart.Breake();

        private void OnFixed() => Fixed?.Invoke(this);

        private void OnFailed() => Failed?.Invoke(this);


        private void OnDestroy()
        {
            _wiresPart.Fixed -= OnFixed;
            _wiresPart.Failed -= OnFailed;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(transform.position, 0.5f);
        }
    }
}