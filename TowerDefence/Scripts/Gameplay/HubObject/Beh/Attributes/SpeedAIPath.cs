using System;
using Gameplay.HubObject.Beh.Effects;
using Pathfinding;
using UnityEngine;

namespace Gameplay.HubObject.Beh.Attributes
{
    [BehaviourButton("Stat/Speed Ai path")]
    public class SpeedAIPath : AbsAttribute
    
    {
        [SerializeField]private AIPath _aiPath;
        [Min(0.1f)][SerializeField] private float _speedBase = 2f;
        
        private float _realSpeed;

        public override IModificateData GetCurrent() => new ModificateData(_speedBase);

        protected override void OnUpdateBuff(IModificateData modificateData)
        {
            _realSpeed = ((ModificateData) modificateData).Speed;
            _aiPath.maxSpeed = _realSpeed;
        }

        public class ModificateData : IModificateData
        {
            public ModificateData(float speed) => Speed = speed;

            public float Speed;
        }
    }
}