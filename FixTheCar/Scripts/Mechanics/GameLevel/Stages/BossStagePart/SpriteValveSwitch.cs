using UnityEngine;

namespace Mechanics.GameLevel.Stages.BossStagePart
{
    public class SpriteValveSwitch : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Sprite _spriteActivated;
        [SerializeField] private Sprite _spriteUnactivated;
        
        public void ChangeToActie() => _spriteRenderer.sprite = _spriteActivated;

        public void ChangeToUnactive() => _spriteRenderer.sprite = _spriteUnactivated;
    }
}