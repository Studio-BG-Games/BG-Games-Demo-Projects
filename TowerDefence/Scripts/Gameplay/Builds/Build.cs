using System;
using Factorys;
using Plugins.DIContainer;
using Plugins.HabObject;
using Plugins.HabObject.CommunicatorParts;
using Plugins.HabObject.GeneralProperty;
using TMPro;
using UnityEngine;

namespace Gameplay.Builds
{
    [RequireComponent(typeof(Plugins.HabObject.ComponentShell))]
    public class Build : HabObject
    {
        private Communicator _communicator;
        private ComponentShell _componentShell;
        private DatasBuild _mainDates;

        public event Action<Build> Destroyd;
        
        public override Communicator Communicator => _communicator!=null?_communicator:_communicator=new Communicator();
        public override ComponentShell ComponentShell => _componentShell!=null?_componentShell:_componentShell= GetComponent<ComponentShell>();
        public override MainDates MainDates => _mainDates!=null?_mainDates:_mainDates=GetComponentInChildren<DatasBuild>();

        private void OnValidate()
        {
            //7 layer "build" for physic
            if (gameObject.layer != 7)
                gameObject.layer = 7;
            if (GetComponent<Collider>() != null)
            {
                var rb = GetComponent<Rigidbody>();
                if (rb == null)
                    rb = gameObject.AddComponent<Rigidbody>();
                rb.isKinematic = true;
                rb.useGravity = false;
            }
        }

        protected override void OnCustonDestroy()
        {
            Destroyd?.Invoke(this);
        }
    }
}