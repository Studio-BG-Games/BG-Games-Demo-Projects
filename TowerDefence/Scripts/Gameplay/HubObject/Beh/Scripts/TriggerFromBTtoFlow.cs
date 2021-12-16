using System;
using Plugins.HabObject;
using UnityEngine;

namespace Gameplay.HubObject.Beh.Scripts
{
    [BehaviourButton("BT/Trigger From BT to Flow")]
    public class TriggerFromBTtoFlow : MonoBehaviour
    {
        public event Action Trigered;
        public event Action<HabObject> TrigeredWithParent;

        [SerializeField] private HabObject _habObject;
        
        public void Activated() => Trigered?.Invoke();
        
        public void ActivatedParent() => TrigeredWithParent?.Invoke(_habObject);
    }
}