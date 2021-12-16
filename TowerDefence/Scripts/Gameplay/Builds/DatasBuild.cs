using Gameplay.Builds.Data;
using Gameplay.HubObject.Data;
using Plugins.HabObject.GeneralProperty;
using UnityEngine;

namespace Gameplay.Builds
{
    [RequireComponent(typeof(Cost), typeof(MaterialBuild),typeof(SizeOnMap))]
    [RequireComponent(typeof(TextureBuild), typeof(TypeBuild), typeof(BanListBrickForBuild))]
    [RequireComponent(typeof(Team), typeof(IdContainer))]
    public class DatasBuild : MainDates
    {
    }
}