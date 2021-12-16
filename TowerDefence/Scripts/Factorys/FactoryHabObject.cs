using System;
using System.Collections.Generic;
using Gameplay.Builds;
using Gameplay.Builds.Data;
using Gameplay.Map;
using Plugins.DIContainer;
using Plugins.HabObject;
using UnityEngine;

namespace Factorys
{
    public abstract class FactoryHabObject<T> : MonoBehaviour where T : HabObject
    {
        [DI] protected WorldShell WorldShell;

        [SerializeField] private Transform _parent;
        
        protected float H;

        public event Action<T> CreatedT;
        
        private bool CanBePlaceAtHere(List<Vector3> pointsForCheck, T template)
        {
            H = -100000;
            foreach (Vector3 position in pointsForCheck)
                if (!CanBePlaceAtHere(position, template, true))
                    return false;
            return true;
        }
        
        public bool CanBePlaceAtHere(T template, Vector3 pos, Vector3 angel)
        {
            H = -100000;
            foreach (Vector3 position in template.MainDates.GetOrNull<SizeOnMap>().GetAllEmployedCell(pos, angel))
                if (!CanBePlaceAtHere(position, template, true))
                    return false;
            return true;
        }

        public bool CanbePlaceAtHereAllPoint(Vector2 posotion, T template)
        {
            var perpos = template.transform.position;
            template.transform.position = new Vector3(posotion.x,0,posotion.y);
            var points = template.MainDates.GetOrNull<SizeOnMap>().GetAllEmployedCell();
            template.transform.position = perpos;
            return CanBePlaceAtHere(points, template);

        }
        
        public void TryMove(T build, Vector3 pos)
        {
            AddActionPreMove(build, pos);            
            var currentPoint = build.MainDates.GetOrNull<SizeOnMap>().GetAllEmployedCell();
            var pointForCheck = build.MainDates.GetOrNull<SizeOnMap>().GetAllEmployedCell(pos, build.transform.eulerAngles);
            MakePointFree(currentPoint);
            if (CanBePlaceAtHere(pointForCheck, build))
            {
                var posBloc = pointForCheck[0];
                posBloc.y = 0;
                pos.y = WorldShell.World.GetBlock(posBloc).GetContent().GetUpestBrick().transform.position.y + 0.5f;
                build.transform.position = pos;
                MakePointBusy(pointForCheck, build);
                AddActiOnMove(build, pos);
            }
            else
            {
                MakePointBusy(currentPoint, build);
            }
        }

        protected virtual void AddActionPreMove(T build, Vector3 pos) { }

        protected virtual void AddActiOnMove(T t, Vector3 pos){}
        
        public void TryMoveDir(T build, Vector3 diraction) => TryMove(build, build.transform.position + diraction);
        
        protected void MakePointFree(List<Vector3> points)
        {
            points.ForEach(x =>
            {
                x.y = 0;
                WorldShell.World.GetBlock(x).GetContent().RemoveHabObject();
            });
        }
        
        protected void MakePointBusy(List<Vector3> points, T newBuild)
        {
            points.ForEach(x =>
            {
                x.y = 0;
                if (!WorldShell.World.GetBlock(x).GetContent().TryAddHabObject(newBuild))
                    throw new Exception("Не удалось добавить здание, посмотри логи.");
            });
        }

        public bool Spawn(T template, Vector3 position, Vector3 angel)
        {
            var pointsForCheck = template.MainDates.GetOrNull<SizeOnMap>().GetAllEmployedCell(position, angel);
            var cost = template.MainDates.GetOrNull<Cost>();
            if (!CanBePlaceAtHere(pointsForCheck, template))
                return false;
            if (!AddDCondtionSpawn(template, position, angel, pointsForCheck))
                return false;
            var result = SpawnT(template, position, angel, pointsForCheck);
            CreatedT?.Invoke(result);
            return true;
        }

        private T SpawnT(T template, Vector3 position, Vector3 angel, List<Vector3> pointsForCheck)
        {
            var newBuild = DiBox.MainBox.CreatePrefab(template);
            position.y = 0;
            var pos = WorldShell.World.GetBlock(position).GetContent().GetUpestBrick().transform.position;
            pos.y += 0.5f;
            newBuild.transform.SetParent(_parent);
            newBuild.transform.position = pos;
            newBuild.transform.eulerAngles = angel;
            MakePointBusy(pointsForCheck, newBuild);
            return newBuild;
        }

        protected abstract bool AddDCondtionSpawn(T template, Vector3 position, Vector3 angel, List<Vector3> pointsForCheck);

        public abstract bool CanBePlaceAtHere(Vector3 position, T template, bool checkHeight = false);

        protected void InvokeEvent(T template) => CreatedT?.Invoke(template);
    }
}