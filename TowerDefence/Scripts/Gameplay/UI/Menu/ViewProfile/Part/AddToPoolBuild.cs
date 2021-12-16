using System.Linq;
using DefaultNamespace.Infrastructure.Data;
using Gameplay.HubObject.Data;

namespace Gameplay.UI.Menu.Canvas
{
    public class AddToPoolBuild : ActionWithPullBuild
    {
        protected override void Action(PlayerBuildProfile profileToView)
        {
            if(!profileToView.CurrentData._isOpen)
                return;
            var id = profileToView.Target.MainDates.GetOrNull<IdContainer>().ID;
            if(_dataGameMono.GetBuildPull().Contains(id))
                return;
            if(_dataGameMono.BuildPullIsFull)
                return;
            _dataGameMono.AddToPullBuild(id);
        }
    }
}