using System;
using Plugins.HabObject;
using Plugins.HabObject.GeneralProperty;
using UnityEngine;

namespace Gameplay.Units
{
    public class TargetContainer : DataProperty
    {
        public HabObject TargetAttack => _targetAttack;
        private HabObject _targetAttack;
        public Vector3 TargetMove => TargetAttack !=null?_targetAttack.transform.position : transform.position;

        public void SetTargetAttack(HabObject habObject) => _targetAttack = habObject;

        public void ZeroTargetAttack() => _targetAttack = null;

        private void OnValidate() => transform.localPosition = Vector3.zero;
    }
}