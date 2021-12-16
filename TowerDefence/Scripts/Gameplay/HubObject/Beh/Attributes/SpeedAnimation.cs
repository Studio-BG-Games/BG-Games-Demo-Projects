using UnityEngine;

namespace Gameplay.HubObject.Beh.Attributes
{
    [BehaviourButton("Other/Speed animation by name")]
    public class SpeedAnimation : MonoBehaviour
    {
        [SerializeField]private Animator _animator;
        [SerializeField] private string _nameProperty;
        [Min(0)][SerializeField] private float _value;
        
        public void SetSpeed() => _animator.SetFloat(_nameProperty, _value);
    }
}