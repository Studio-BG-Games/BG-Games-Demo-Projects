using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Gameplay.Builds;
using Gameplay.Builds.Data;
using Gameplay.GameSceneScript;
using Gameplay.Map;
using Plugins.DIContainer;
using Plugins.HabObject.GeneralProperty;
using UnityEngine;

namespace Factorys
{
    public class FactoryBuild : FactoryHabObject<Build>
    {
        [DI] private IGold _gold;

        protected override bool AddDCondtionSpawn(Build template, Vector3 position, Vector3 angel, List<Vector3> pointsForCheck) 
            => _gold.TryRemove(template.MainDates.GetOrNull<Cost>().Gold);

        public override bool CanBePlaceAtHere(Vector3 position, Build template, bool checkHeight = false)
        {
            var selfPoint = position;
            selfPoint.y = 0;
            if (!WorldShell.World.IsOccupied(selfPoint))
            {
                return false;
            }
            var contetn = WorldShell.World.GetBlock(selfPoint).GetContent();
            var upestBrick = contetn.GetUpestBrick();
            if (H == -100000 && checkHeight)
                H = upestBrick.transform.position.y;
            if (contetn == null)
            {
                return false;
            }

            if (upestBrick.transform.position.y != H && checkHeight)
            {
                return false;
            }
            foreach (var brick in template.MainDates.UnsafeGetOrNull<BanListBrickForBuild>().Bricks)
                if (upestBrick.ID == brick.ID)
                {
                    return false;
                }

            if (contetn.Hab != null)
            {
                return false;
            }
            return true;
        }
    }
}