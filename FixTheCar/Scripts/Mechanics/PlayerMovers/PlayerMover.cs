using UnityEngine;

namespace Mechanics
{
    public abstract class PlayerMover : MonoBehaviour
    {
        [SerializeField] protected Collider2D Collider;
        [SerializeField] protected Rigidbody2D Rigidbody2D;
        [Min(0)][SerializeField] protected float Speed;
        [SerializeField] protected Player Player;
        [SerializeField] protected SpriteRenderer SpriteRenderer;

        public virtual void On()
        {
            Rigidbody2D.simulated = Collider.enabled = true;
        }

        public virtual void Off()
        {
            Rigidbody2D.simulated = Collider.enabled = false;
            Player.SetMoveAnimationActive(false);
            SpriteRenderer.flipX = false;
        }

        public abstract void SetFakeStop(bool isStop);
    }
}