using DefaultNamespace.Infrastructure.Data;
using Gameplay.HubObject.Data;

namespace Gameplay.UI.Menu.Canvas
{
    public class RemoveFromPoolBuild : ActionWithPullBuild
    {
        protected override void Action(PlayerBuildProfile profileToView)
        {
            _dataGameMono.RemoveFromPullBuild(profileToView.Target.MainDates.GetOrNull<IdContainer>().ID);
        }
    }
}