using Gameplay.Builds.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.UI
{
    public class InitCost : PartInitConstructButton
    {
        [SerializeField] private Text _label;
        
        public override void Init<T>(T template)
        {
            _label.text = template.MainDates.TryGet<Cost>(out var cost) ? cost.Gold.ToString() : "free";
        }
    }
}