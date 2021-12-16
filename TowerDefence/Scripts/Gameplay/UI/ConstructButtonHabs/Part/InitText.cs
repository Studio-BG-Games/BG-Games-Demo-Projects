using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.UI
{
    public class InitText : PartInitConstructButton
    {
        [SerializeField] private Text _label;
        
        public override void Init<T>(T template)
        {
            _label.text = template.name;
        }
    }
}