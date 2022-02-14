using UnityEngine;

namespace Baby_vs_Aliens
{
    public interface IProjectile : IDamageDealer
    {
        public void Init(Vector3 startingPosition, Vector3 moveDirection, ProjectileConfig config);
    }
}