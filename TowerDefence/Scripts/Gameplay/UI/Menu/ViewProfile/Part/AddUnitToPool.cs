using System.Linq;
using DefaultNamespace.Infrastructure.Data;
using DefaultNamespace.Infrastructure.Data.ParstOfCardProfiles;
using Gameplay.HubObject.Data;

namespace Gameplay.UI.Menu.Canvas
{
    public class AddUnitToPool : ActionWithPullUnit
    {
       protected override void Action(PlayerUnitProfile playerUnitProfile, string id)
        {
            if(!playerUnitProfile.CurrentData._isOpen)
                return;
            if(_dataGameMono.GetUnitPull().Contains(id))
                return;

            if(_dataGameMono.UnitPullIsFull)
                return;
            
            _dataGameMono.AddToPullUnitNewElelment(id);
            _dataGameMono.SaveCurrentData();
        }
    }
}