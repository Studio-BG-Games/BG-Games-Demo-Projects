using System;
using BehTreeBrick.Action;
using Plugins.HabObject;

namespace Gameplay.HubObject.Beh.GiverTarget.PartSearchInTarget
{
    public interface IPartSearchRadius
    {
        SearcherTargetInRadius.ResultCheck IsCorrect(HabObject target);
        IPartSearchRadius Clone(HabObject parrent);
    }
}