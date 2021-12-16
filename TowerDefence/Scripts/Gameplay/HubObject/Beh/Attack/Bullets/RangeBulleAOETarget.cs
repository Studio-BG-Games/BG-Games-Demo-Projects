using System;
using System.Collections.Generic;
using BehTreeBrick.Action;
using Plugins.HabObject;
using UnityEngine;

namespace Gameplay.HubObject.Beh.Attack
{
    public class RangeBulleAOETarget : AbsBullet
    {
        [SerializeField] private SearcherTargetInRadius _searcherTargetInRadius;

        private void Start() => _searcherTargetInRadius.Init(Damage.HabObject);

        protected override List<HabObject> GetTargets() => _searcherTargetInRadius.All();
    }
}