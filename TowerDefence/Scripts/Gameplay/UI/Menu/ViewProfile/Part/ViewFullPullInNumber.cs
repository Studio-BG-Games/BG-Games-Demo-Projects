using DefaultNamespace.Infrastructure.Data;
using DefaultNamespace.Infrastructure.Data.ParstOfCardProfiles;
using Plugins.DIContainer;
using TMPro;
using UnityEngine;

namespace Gameplay.UI.Menu.Canvas
{
    public class ViewFullPullInNumber : ViewPartProfile
    {
        [SerializeField] private TextMeshProUGUI _label;

        [DI] private DataGameMono _dataGameMono;
        
        public override void View<T, TData>(ObjectCardProfile<T, TData> profileToView)
        {
            if (profileToView is PlayerUnitProfile)
            {
                _label.text = $"{_dataGameMono.GetUnitPull().Length}/{_dataGameMono.MaxCountUnit}";
            }
            else if (profileToView is PlayerBuildProfile)
            {
                _label.text = $"{_dataGameMono.GetBuildPull().Length}/{_dataGameMono.MaxCountBuild}";
            }
            else
            {
                _label.text = "";
            }
        }
    }
}