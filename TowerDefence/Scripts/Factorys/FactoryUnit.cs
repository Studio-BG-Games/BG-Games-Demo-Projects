using System;
using System.Collections.Generic;
using Gameplay.Builds.Data;
using Gameplay.GameSceneScript;
using Gameplay.HubObject.Data;
using Gameplay.Map;
using Gameplay.Units;
using Gameplay.Units.Beh;
using Plugins.DIContainer;
using UnityEngine;

namespace Factorys
{
    public class FactoryUnit : FactoryHabObject<Unit>
    {
        [DI] private WorldShell _worldShell;
        [DI] private IGold _gold;

        [HideInInspector] public bool ToCheckCost = true;

        private void Start() => ToCheckCost = true;

        public Unit SpawnEnemy(Unit unit, Transform point)
        {
            var result = DiBox.MainBox.CreatePrefab(unit);
            var p = point.position;
            p.y = 0;
            var pos = _worldShell.World.GetBlock(p).GetContent().GetUpestBrick().transform.position;
            pos.y += 0.75f;
            result.transform.position = pos;
            result.transform.eulerAngles = new Vector3(0,point.eulerAngles.y,0);
            InvokeEvent(result);
            return result;
        }

        protected override void AddActiOnMove(Unit t, Vector3 pos)
        {
            t.ComponentShell.GetAll<ZoneMove>().ForEach(x=>x.UpdateCenter());
        }

        protected override bool AddDCondtionSpawn(Unit template, Vector3 position, Vector3 angel, List<Vector3> pointsForCheck)
        {
            var cost = template.MainDates.GetOrNull<Cost>();
            if(cost && ToCheckCost)
                if (!_gold.TryRemove(cost.Gold))
                    return false;
            return true;
        }

        protected override void AddActionPreMove(Unit build, Vector3 pos)
        {
            if (build.MainDates.GetOrNull<Team>().Type != Team.Typ.Player)
            {
                throw  new Exception($"{build.name} враг и его нельзя двигать, у вас что-то не так");
            }
            else
            {
                var zoneMove = build.ComponentShell.Get<ZoneMove>();
                if(!zoneMove)
                    throw  new Exception($"{build.name} игрок, но не имеет поведения Zone Move, FIx it");
                build.transform.position = zoneMove.Center;
                build.transform.eulerAngles = zoneMove.Rotate;
            }
        }

        public override bool CanBePlaceAtHere(Vector3 position, Unit template, bool checkHeight = false)
        {
            position.y = 0;
            if (!_worldShell.World.IsOccupied(position))
            {
                return false;
            }

            if (_worldShell.World.GetBlock(position).GetContent().Hab != null)
            {
                return false;
            }

            return true;
        }
    }
}