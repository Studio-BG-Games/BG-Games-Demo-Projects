using System;
using Gameplay.Builds;
using MaxyGames.uNode;
using Pathfinding;
using Plugins.HabObject;
using Plugins.HabObject.CommunicatorParts;
using Plugins.HabObject.GeneralProperty;
using UnityEngine;

namespace Gameplay.Units
{
    [RequireComponent(typeof(Plugins.HabObject.ComponentShell), typeof(AIPath), typeof(RaycastModifier))]
    [RequireComponent(typeof(uNodeSpawner), typeof(CharacterController), typeof(AlternativePath))]
    [RequireComponent(typeof(AiPathShell))]
    public class Unit : HabObject
    {
        private Communicator _communicator;
        private ComponentShell _componentShell;
        private DatasUnit _mainDates;
        public override Communicator Communicator => _communicator!=null?_communicator:_communicator=new Communicator();
        public override ComponentShell ComponentShell => _componentShell!=null?_componentShell:_componentShell= GetComponent<ComponentShell>();
        public override MainDates MainDates => _mainDates!=null?_mainDates:_mainDates=GetComponentInChildren<DatasUnit>();

        private void OnValidate()
        {
            if (gameObject.layer != 8)
                gameObject.layer = 8;
            var ctrl = GetComponent<CharacterController>();
            if (ctrl.stepOffset < 0.65f)
                ctrl.stepOffset = 0.65f;
            if (ctrl.slopeLimit != 90)
                ctrl.slopeLimit = 90;
        }
    }
}