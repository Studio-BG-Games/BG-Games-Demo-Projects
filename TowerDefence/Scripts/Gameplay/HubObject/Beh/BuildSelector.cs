using System;
using Gameplay.Builds;
using Plugins.HabObject;
using UnityEngine;

namespace Gameplay
{
    [BehaviourButton("Other/Build selector")]
    public class BuildSelector : SelectorHabObject
    {
        private Build Build => _build ? _build : _build = GetComponentInParent<Build>();
        private Build _build;
        
        public override HabObject Get() => Build;
        
        [ContextMenu("Test me")]
        private void Test()
        {
            Debug.Log(Get());
        }
    }
}