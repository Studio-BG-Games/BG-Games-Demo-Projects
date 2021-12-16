using DefaultNamespace.Infrastructure.Data;
using DefaultNamespace.Infrastructure.Data.ParstOfCardProfiles;
using TMPro;
using UnityEngine;

namespace Gameplay.UI.Menu.Canvas
{
    public class ViewCost : ViewPartProfile
    {
        [SerializeField] private TextMeshProUGUI _label;
        
        public override void View<T, TData>(ObjectCardProfile<T, TData> profileToView)
        {
            var cost = profileToView.GetFirstOrNull<CostPart>();
            if (!cost)
            {
                _label.text = "";
                return;
            }
            bool isBuy = false;
            if (profileToView.CurrentData is PlayerUnitData)
                isBuy = (profileToView.CurrentData as PlayerUnitData)._isOpen;
            else
                isBuy = (profileToView.CurrentData as PlayerBuildData)._isOpen;

            if (!isBuy)
                _label.text = cost.Cost.ToString();
            else
                _label.text = "";
        }
        
        
    }
}