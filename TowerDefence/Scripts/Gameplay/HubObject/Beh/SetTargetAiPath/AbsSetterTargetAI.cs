using System;
using Gameplay.Units;
using Pathfinding;
using UnityEngine;

namespace Gameplay
{
    public abstract class AbsSetterTargetAI : MonoBehaviour
    {
        [SerializeField] protected Unit Unit;
        private AIPath AiPath=>_aiPath?_aiPath:_aiPath=Unit.GetComponent<AIPath>();
        private AIPath _aiPath;
        public void SetPath() => AiPath.destination = GetDestination();
        public abstract Vector3 GetDestination();
    }
}