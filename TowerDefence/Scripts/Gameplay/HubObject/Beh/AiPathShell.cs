using Pathfinding;
using UnityEngine;

namespace Gameplay
{
    public class AiPathShell : MonoBehaviour
    {
        [SerializeField] private AIPath _aiPath;

        public void On() => _aiPath.enabled = true;
        public void Off() => _aiPath.enabled = false;
        public void SetDestention(Vector3 target) => _aiPath.destination = target;
    }
}