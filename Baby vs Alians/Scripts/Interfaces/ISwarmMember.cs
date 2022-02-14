using UnityEngine;

namespace Baby_vs_Aliens
{
    public interface ISwarmMember : IEnemy
    {
        bool IsSwarmLeader { get; set; }
        Vector3 LeaderPosition { get; }
        Vector3 Position { get; }

        Vector3 GetRandomPositionAroundLeader();
    }
}