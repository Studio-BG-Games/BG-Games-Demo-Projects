using System;
using Mechanics.Interfaces;
using UnityEngine;

namespace Mechanics.GameLevel.Stages
{
    public abstract class Stage : MonoBehaviour
    {
        public Action Completed;
        public abstract SizeElement SizeElement { get; }
        public abstract void StartStage(Player player, bool isInstantaneousTransit);
        public abstract void Init(StageData stageData);
    }
}