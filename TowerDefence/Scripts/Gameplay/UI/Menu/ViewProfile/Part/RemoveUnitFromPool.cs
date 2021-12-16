using System.Linq;
using DefaultNamespace.Infrastructure.Data;
using DefaultNamespace.Infrastructure.Data.ParstOfCardProfiles;

namespace Gameplay.UI.Menu.Canvas
{
    public class RemoveUnitFromPool : ActionWithPullUnit
    {
       protected override void Action(PlayerUnitProfile playerUnitProfile, string id)
        {
            if(!playerUnitProfile.CurrentData._isOpen)
                return;
            if (id == null)
                return;

            if(!_dataGameMono.GetUnitPull().Contains(id))
                return;
            
            _dataGameMono.RemovePlayerPullElement(id);
            _dataGameMono.SaveCurrentData();
        }
    }
}