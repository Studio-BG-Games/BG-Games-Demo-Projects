using UnityEngine;

namespace Gameplay.Builds.Beh
{
    public class FixedRotateble : MonoBehaviour
    {
        [SerializeField] private Transform Target;

        public void SetZero()
        {
            Target.eulerAngles=Vector3.zero;
        }
        
        public void RotateRight()
        {
            if(Target.eulerAngles.y >= 270)
                SetZero();
            else
                Target.eulerAngles+=new Vector3Int(0,90,0);
        }

        public void RotateLeft()
        {
            Target.eulerAngles-=new Vector3Int(0,90,0);
        }
    }
}