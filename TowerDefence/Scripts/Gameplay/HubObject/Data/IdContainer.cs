using System;
using Plugins.HabObject;
using Plugins.HabObject.GeneralProperty;
using UnityEngine;

namespace Gameplay.HubObject.Data
{
    public class IdContainer : DataProperty
    {
        [SerializeField]private HabObject _habObjecth;
        [SerializeField][HideInInspector]private string _id;

        public string ID => _id;

        public void DebugMe()
        {
            Debug.Log($"{_habObjecth}, name = {_habObjecth .name}, id = {_id}");
        }
        
        private void OnValidate()
        {
            _habObjecth = transform.parent.GetComponent<HabObject>();
            if(_habObjecth==null)
                Debug.LogWarning($"{name} dont hab parent hab object");
        }
    }
}