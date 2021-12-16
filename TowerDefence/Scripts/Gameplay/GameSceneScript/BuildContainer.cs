using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Gameplay;
using Gameplay.Builds;
using Gameplay.Builds.Data.Marks;
using Gameplay.HubObject.Beh;
using Gameplay.HubObject.Beh.Attributes;
using Plugins.DIContainer;
using Plugins.HabObject;
using UnityEngine;

namespace Factorys
{
    public class BuildContainer : MonoBehaviour
    {
        public event Action<Build> SomeBuildDead;
        public event Action<Build> NeksusDead;
        
        
        private List<Build> _builds = new List<Build>();
        public List<Build> Builds => _builds;

        public Build Neksus => _neksus;
        private Build _neksus;

        [DI] private FactoryBuild _factoryBuild;

        private void Awake() => _factoryBuild.CreatedT += OnBuildCreated;

        private void OnBuildCreated(Build obj)
        {
            if(obj.MainDates.GetOrNull<PropMark>()) return;
            if (obj.MainDates.GetOrNull<NeksusMark>()) _neksus = obj;
            _builds.Add(obj);
            var health = obj.ComponentShell.Get<Health>();
            if(health)
                health.Dead += OnDead;
            var desBah = obj.ComponentShell.Get<DestroyObject>();
            if (desBah)
                desBah.DestroyByCallback += OnDead;
            obj.Destroyd += OnDead;
        }

        private void OnDead(HabObject obj)
        {
            if(!obj)
                return;
            var build = (Build) obj;
            _builds.Remove(build);
            if (obj.MainDates.GetOrNull<NeksusMark>()) NeksusDead?.Invoke(build);
            SomeBuildDead?.Invoke(build);
        }
    }
}