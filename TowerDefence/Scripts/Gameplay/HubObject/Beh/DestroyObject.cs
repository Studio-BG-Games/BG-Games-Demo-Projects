using System;
using Gameplay.HubObject.Beh.Scripts;
using Plugins.HabObject;
using UnityEngine;

namespace Gameplay
{
    [BehaviourButton("Other/Destoryer")]
    public class DestroyObject : MonoBehaviour
    {
        [SerializeField] private HabObject _habObject;

        public event Action<HabObject> DestroyByMethod;
        public event Action<HabObject> DestroyByCallback;

        public void Destroy()
        {
            DestroyByMethod?.Invoke(_habObject);
            Destroy(_habObject.gameObject);
        }

        private void OnDestroy()
        {
            DestroyByCallback?.Invoke(_habObject);
        }

        private void OnValidate()
        {
            _habObject = GetComponentInParent<HabObject>();
            if(!_habObject)
                Debug.LogError($"{name} не имеет Hab object для удаления");
        }
    }
}